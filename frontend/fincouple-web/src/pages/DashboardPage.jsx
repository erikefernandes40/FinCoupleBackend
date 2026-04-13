import { useAuth } from '../context/AuthContext';

export default function DashboardPage({ onNavigate }) {
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    onNavigate('login');
  };

  return (
    <div style={{ padding: 24 }}>
      <h1>FinCouple Dashboard</h1>
      <p>Welcome, <strong>{user?.name}</strong>!</p>
      <nav style={{ display: 'flex', gap: 12, marginBottom: 24 }}>
        <button onClick={() => onNavigate('expenses')}>Expenses</button>
        <button onClick={() => onNavigate('recurring')}>Recurring Expenses</button>
        <button onClick={() => onNavigate('budgets')}>Budgets</button>
        <button onClick={handleLogout} style={{ marginLeft: 'auto' }}>Logout</button>
      </nav>
      <p>Use the navigation above to manage your couple's finances.</p>
    </div>
  );
}
