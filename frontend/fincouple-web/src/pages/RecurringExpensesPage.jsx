import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import api from '../api/axiosInstance';

export default function RecurringExpensesPage({ onNavigate }) {
  const { user } = useAuth();
  const coupleId = user?.coupleId;
  const [items, setItems] = useState([]);
  const [categories, setCategories] = useState([]);
  const [form, setForm] = useState({ description: '', amount: '', categoryId: '', recurrenceType: 'Monthly', dayOfMonth: 1 });
  const [error, setError] = useState('');

  useEffect(() => {
    if (!coupleId) return;
    api.get(`/api/recurringexpenses/couple/${coupleId}`).then(({ data }) => setItems(data)).catch(() => {});
    api.get('/api/categories').then(({ data }) => setCategories(data)).catch(() => {});
  }, [coupleId]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const now = new Date();
      const day = parseInt(form.dayOfMonth, 10);
      const nextDueDate = new Date(now.getFullYear(), now.getMonth() + 1, day).toISOString();
      const payload = {
        coupleId,
        categoryId: form.categoryId || (categories[0]?.id ?? '00000000-0000-0000-0000-000000000000'),
        createdByUserId: user.userId,
        description: form.description,
        amount: parseFloat(form.amount),
        recurrenceType: form.recurrenceType,
        dayOfMonth: day,
        nextDueDate,
      };
      const { data } = await api.post('/api/recurringexpenses', payload);
      setItems((prev) => [data, ...prev]);
      setForm({ description: '', amount: '', categoryId: '', recurrenceType: 'Monthly', dayOfMonth: 1 });
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to create recurring expense');
    }
  };

  const handleToggle = async (id) => {
    const { data } = await api.patch(`/api/recurringexpenses/${id}/toggle`);
    setItems((prev) => prev.map((i) => i.id === id ? data : i));
  };

  return (
    <div style={{ padding: 24 }}>
      <button onClick={() => onNavigate('dashboard')}>← Back</button>
      <h2>Recurring Expenses</h2>
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
          <select name="recurrenceType" value={form.recurrenceType} onChange={handleChange} style={{ padding: 8 }}>
            <option value="Monthly">Monthly</option>
            <option value="Weekly">Weekly</option>
            <option value="Yearly">Yearly</option>
          </select>
          <input name="dayOfMonth" type="number" min="1" max="31" value={form.dayOfMonth} onChange={handleChange} style={{ padding: 8, width: 80 }} placeholder="Day" />
          <button type="submit" style={{ padding: '8px 16px', background: '#4f46e5', color: '#fff', border: 'none', borderRadius: 4 }}>Add</button>
        </form>
      )}
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ background: '#f3f4f6' }}>
            <th style={{ padding: 8, textAlign: 'left' }}>Description</th>
            <th style={{ padding: 8, textAlign: 'right' }}>Amount</th>
            <th style={{ padding: 8 }}>Recurrence</th>
            <th style={{ padding: 8 }}>Next Due</th>
            <th style={{ padding: 8 }}>Status</th>
            <th style={{ padding: 8 }}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <tr key={item.id} style={{ borderBottom: '1px solid #e5e7eb' }}>
              <td style={{ padding: 8 }}>{item.description}</td>
              <td style={{ padding: 8, textAlign: 'right' }}>R$ {Number(item.amount).toFixed(2)}</td>
              <td style={{ padding: 8, textAlign: 'center' }}>{item.recurrenceType}</td>
              <td style={{ padding: 8 }}>{new Date(item.nextDueDate).toLocaleDateString()}</td>
              <td style={{ padding: 8, textAlign: 'center' }}>
                <span style={{ color: item.isActive ? '#22c55e' : '#ef4444' }}>{item.isActive ? 'Active' : 'Inactive'}</span>
              </td>
              <td style={{ padding: 8, textAlign: 'center' }}>
                <button onClick={() => handleToggle(item.id)} style={{ padding: '4px 8px', border: 'none', borderRadius: 4, cursor: 'pointer', background: item.isActive ? '#f59e0b' : '#22c55e', color: '#fff' }}>
                  {item.isActive ? 'Deactivate' : 'Activate'}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
