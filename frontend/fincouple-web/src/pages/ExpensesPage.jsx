import { useEffect, useMemo, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useSSE } from '../hooks/useSSE';
import api from '../api/axiosInstance';

export default function ExpensesPage({ onNavigate }) {
  const { user } = useAuth();
  const coupleId = user?.coupleId;
  const { expenses: sseExpenses } = useSSE(coupleId);
  const [fetchedExpenses, setFetchedExpenses] = useState([]);
  const [form, setForm] = useState({ description: '', amount: '', categoryId: '', date: new Date().toISOString().split('T')[0] });
  const [categories, setCategories] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!coupleId) return;
    api.get(`/api/expenses/couple/${coupleId}`).then(({ data }) => setFetchedExpenses(data)).catch(() => {});
    api.get('/api/categories').then(({ data }) => setCategories(data)).catch(() => {});
  }, [coupleId]);

  // Merge fetched + SSE expenses, deduplicating by id
  const expenses = useMemo(() => {
    const map = new Map(fetchedExpenses.map((e) => [e.id, e]));
    for (const exp of sseExpenses) {
      if (!map.has(exp.id)) map.set(exp.id, exp);
    }
    return [...map.values()].sort((a, b) => new Date(b.date) - new Date(a.date));
  }, [fetchedExpenses, sseExpenses]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const payload = {
        coupleId,
        categoryId: form.categoryId || (categories[0]?.id ?? '00000000-0000-0000-0000-000000000000'),
        paidByUserId: user.userId,
        description: form.description,
        amount: parseFloat(form.amount),
        date: new Date(form.date).toISOString(),
        isRecurring: false,
      };
      const { data } = await api.post('/api/expenses', payload);
      setFetchedExpenses((prev) => [data, ...prev]);
      setForm({ description: '', amount: '', categoryId: '', date: new Date().toISOString().split('T')[0] });
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to create expense');
    }
  };

  const handleDelete = async (id) => {
    await api.delete(`/api/expenses/${id}`);
    setFetchedExpenses((prev) => prev.filter((e) => e.id !== id));
  };

  return (
    <div style={{ padding: 24 }}>
      <button onClick={() => onNavigate('dashboard')}>← Back</button>
      <h2>Expenses</h2>
      {!coupleId && <p style={{ color: 'orange' }}>You are not linked to a couple yet.</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {coupleId && (
        <form onSubmit={handleSubmit} style={{ display: 'flex', gap: 8, marginBottom: 24, flexWrap: 'wrap' }}>
          <input name="description" placeholder="Description" value={form.description} onChange={handleChange} required style={{ padding: 8, flex: 1 }} />
          <input name="amount" type="number" step="0.01" placeholder="Amount" value={form.amount} onChange={handleChange} required style={{ padding: 8, width: 120 }} />
          <select name="categoryId" value={form.categoryId} onChange={handleChange} style={{ padding: 8 }}>
            <option value="">Select category</option>
            {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
          </select>
          <input type="date" name="date" value={form.date} onChange={handleChange} required style={{ padding: 8 }} />
          <button type="submit" style={{ padding: '8px 16px', background: '#4f46e5', color: '#fff', border: 'none', borderRadius: 4 }}>Add</button>
        </form>
      )}
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ background: '#f3f4f6' }}>
            <th style={{ padding: 8, textAlign: 'left' }}>Description</th>
            <th style={{ padding: 8, textAlign: 'right' }}>Amount</th>
            <th style={{ padding: 8, textAlign: 'left' }}>Date</th>
            <th style={{ padding: 8 }}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {expenses.map((e) => (
            <tr key={e.id} style={{ borderBottom: '1px solid #e5e7eb' }}>
              <td style={{ padding: 8 }}>{e.description}</td>
              <td style={{ padding: 8, textAlign: 'right' }}>R$ {Number(e.amount).toFixed(2)}</td>
              <td style={{ padding: 8 }}>{new Date(e.date).toLocaleDateString()}</td>
              <td style={{ padding: 8, textAlign: 'center' }}>
                <button onClick={() => handleDelete(e.id)} style={{ background: '#ef4444', color: '#fff', border: 'none', borderRadius: 4, padding: '4px 8px', cursor: 'pointer' }}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
