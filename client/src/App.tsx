import { useEffect, useState } from 'react'
import './App.css'

interface Person {
    name: string;
    age: number;
    sex: string;
    car: Car;
}

interface Car {
    model: string;
    plate: string;
    volume: number;
    createYear: number;
}

function App() {
    const [mode, setMode] = useState<1 | 2>(1);//(1) - значение по умолчанию

    const [person, setPerson] = useState<Person | null>(null);
    const [people, setPeople] = useState<Person[]>([]);

    useEffect(() => {
        if (mode === 1) {
            fetch("https://localhost:7094/api/person")
                .then(r => r.json())
                .then(data => setPerson(data))
                .catch(console.error);
        }

        if (mode === 2) {
            fetch("https://localhost:7094/api/personList")
                .then(r => r.json())
                .then(data => setPeople(data))
                .catch(console.error);
        }
    }, [mode]);

    <button onClick={() => setMode(mode === 1 ? 2 : 1)}>
        Режим: {mode}
    </button>

    return (
        <div>
            <button onClick={() => setMode(mode === 1 ? 2 : 1)}>
                Режим: {mode}
            </button>

            {mode === 1 && person && (
                <>
                    <h3>Чувачело</h3>
                    <p>Имя: {person.name}</p>
                    <p>Возраст: {person.age}</p>
                    <p>Пол: {person.sex}</p>

                    <h3>Его тачила</h3>
                    <p>Модель: {person.car.model}</p>
                    <p>Номер: {person.car.plate}</p>
                    <p>Объём: {person.car.volume}</p>
                    <p>Год: {person.car.createYear}</p>
                </>
            )}

            {mode === 2 && people.map((p, i) => (
                <div key={i}>
                    <h3>Чувачело {i}</h3>
                    <p>Имя: {p.name}</p>
                    <p>Возраст: {p.age}</p>
                    <p>Пол: {p.sex}</p>

                    <h3>Его тачила</h3>
                    <p>{p.car.model}</p>
                    <p>{p.car.plate}</p>
                    <p>{p.car.volume}</p>
                    <p>{p.car.createYear}</p>

                    <hr />
                </div>
            ))}
        </div>
    );
}

export default App;