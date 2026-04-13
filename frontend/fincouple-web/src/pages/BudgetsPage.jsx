import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import api from '../api/axiosInstance';

export default function BudgetsPage({ onNavigate }) {
  const { user } = useAuth();
  const coupleId = user?.coupleId;
  const now = new Date();
  const [month, setMonth] = useState(now.getMonth() + 1);
  const [year, setYear] = useState(now.getFullYear());
  const [budgets, setBudgets] = useState([]);
  const [categories, setCategories] = useState([]);
  const [form, setForm] = useState({ categoryId: '', limitAmount: '' });
  const [error, setError] = useState('');

  useEffect(() => {
    if (!coupleId) return;
    api.get(`/api/budgets/couple/${coupleId}?month=${month}&year=${year}`).then(({ data }) => setBudgets(data)).catch(() => {});
    api.get('/api/categories').then(({ data }) => setCategories(data)).catch(() => {});
  }, [coupleId, month, year]);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const payload = {
        coupleId,
        categoryId: form.categoryId || (categories[0]?.id ?? '00000000-0000-0000-0000-000000000000'),
        limitAmount: parseFloat(form.limitAmount),
        month: parseInt(month, 10),
        year: parseInt(year, 10),
      };
      const { data } = await api.post('/api/budgets', payload);
      setBudgets((prev) => {
        const idx = prev.findIndex((b) => b.id === data.id);
        if (idx >= 0) { const arr = [...prev]; arr[idx] = data; return arr; }
        return [...prev, data];
      });
      setForm({ categoryId: '', limitAmount: '' });
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to upsert budget');
    }
  };

  return (
    <div style={{ padding: 24 }}>
      <button onClick={() => onNavigate('dashboard')}>← Back</button>
      <h2>Budgets</h2>
      {!coupleId && <p style={{ color: 'orange' }}>You are not linked to a couple yet.</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <div style={{ display: 'flex', gap: 8, marginBottom: 16 }}>
        <label>Month: <input type="number" min="1" max="12" value={month} onChange={(e) => setMonth(Number(e.target.value))} style={{ width: 60, padding: 8 }} /></label>
        <label>Year: <input type="number" min="2000" max="2100" value={year} onChange={(e) => setYear(Number(e.target.value))} style={{ width: 80, padding: 8 }} /></label>
      </div>
      {coupleId && (
        <form onSubmit={handleSubmit} style={{ display: 'flex', gap: 8, marginBottom: 24, flexWrap: 'wrap' }}>
          <select name="categoryId" value={form.categoryId} onChange={handleChange} style={{ padding: 8, flex: 1 }}>
            <option value="">Select category</option>
            {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
          </select>
          <input name="limitAmount" type="number" step="0.01" placeholder="Limit Amount" value={form.limitAmount} onChange={handleChange} required style={{ padding: 8, width: 160 }} />
          <button type="submit" style={{ padding: '8px 16px', background: '#4f46e5', color: '#fff', border: 'none', borderRadius: 4 }}>Set Budget</button>
        </form>
      )}
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ background: '#f3f4f6' }}>
            <th style={{ padding: 8, textAlign: 'left' }}>Category</th>
            <th style={{ padding: 8, textAlign: 'right' }}>Limit</th>
          </tr>
        </thead>
        <tbody>
          {budgets.map((b) => (
            <tr key={b.id} style={{ borderBottom: '1px solid #e5e7eb' }}>
              <td style={{ padding: 8 }}>{categories.find((c) => c.id === b.categoryId)?.name || b.categoryId}</td>
              <td style={{ padding: 8, textAlign: 'right' }}>R$ {Number(b.limitAmount).toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
