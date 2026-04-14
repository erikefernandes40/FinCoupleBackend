import { useState } from 'react';
import { AuthProvider, useAuth } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import ExpensesPage from './pages/ExpensesPage';
import RecurringExpensesPage from './pages/RecurringExpensesPage';
import BudgetsPage from './pages/BudgetsPage';

function AppRouter() {
  const { token } = useAuth();
  const [page, setPage] = useState(token ? 'dashboard' : 'login');

  if (!token && page !== 'login') {
    return <LoginPage onNavigate={setPage} />;
  }

  switch (page) {
    case 'login': return <LoginPage onNavigate={setPage} />;
    case 'dashboard': return <DashboardPage onNavigate={setPage} />;
    case 'expenses': return <ExpensesPage onNavigate={setPage} />;
    case 'recurring': return <RecurringExpensesPage onNavigate={setPage} />;
    case 'budgets': return <BudgetsPage onNavigate={setPage} />;
    default: return <DashboardPage onNavigate={setPage} />;
  }
}

export default function App() {
  return (
    <AuthProvider>
      <AppRouter />
    </AuthProvider>
  );
}
