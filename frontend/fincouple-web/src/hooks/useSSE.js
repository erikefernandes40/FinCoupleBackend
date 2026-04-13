import { useEffect, useRef, useState } from 'react';
import { useAuth } from '../context/AuthContext';

export function useSSE(coupleId) {
  const { token } = useAuth();
  const [expenses, setExpenses] = useState([]);
  const esRef = useRef(null);

  useEffect(() => {
    if (!coupleId || !token) return;

    const url = `${import.meta.env.VITE_API_URL || 'http://localhost:5000'}/api/sse/stream?coupleId=${coupleId}`;
    const es = new EventSource(url);
    esRef.current = es;

    es.onmessage = (event) => {
      try {
        const expense = JSON.parse(event.data);
        setExpenses((prev) => {
          const exists = prev.find((e) => e.id === expense.id);
          if (exists) return prev;
          return [expense, ...prev];
        });
      } catch {
        // ignore parse errors
      }
    };

    es.onerror = () => {
      es.close();
    };

    return () => {
      es.close();
    };
  }, [coupleId, token]);

  return { expenses, setExpenses };
}
