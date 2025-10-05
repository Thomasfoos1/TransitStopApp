import React, { useEffect, useState } from "react";
const apiUrl = import.meta.env.VITE_API_URL;

export default function App() {
    const [stops, setStops] = useState([]);
    const [selected, setSelected] = useState("");
    const [next, setNext] = useState(null);

    useEffect(() => {
        fetch(`${apiUrl}/api/stops`)
            .then(r => r.json())
            .then(setStops);
    }, []);

    async function clickStop(id) {
        const r = await fetch(`${apiUrl}/api/stops/${id}/next`);
        if (!r.ok) {
            setNext("No schedule");
            return;
        }
        const json = await r.json();
        setNext(json.nextStop);
        setSelected(id);
    }

    return (
        <div style={{ padding: 16, fontFamily: "Arial, sans-serif" }}>
            <h3>Route F Stops</h3>

            <select
                value={selected}
                onChange={(e) => clickStop(e.target.value)}
                style={{ padding: 4, fontSize: 14 }}
            >
                <option value="">-- Select a stop --</option>
                {stops.map(s => (
                    <option key={s.id} value={s.id}>
                        {s.stopOrder}. {s.name}
                    </option>
                ))}
            </select>

            <div style={{ marginTop: 16 }}>
                {selected ? (
                    <div>Next at: <strong>{next}</strong></div>
                ) : (
                    <div>Pick a stop</div>
                )}
            </div>
        </div>
    );
}