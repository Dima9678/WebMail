import http from 'k6/http';

const BASE_URL = 'https://localhost:7094';

export const options = {
    scenarios: {
        load_test: {
            executor: 'constant-arrival-rate',
            rate: 200, //итераций в секунду
            timeUnit: '1s',
            duration: '10m', //тест идет в течении секунд
            preAllocatedVUs: 200, // создание виртуальных пользователей
            maxVUs: 400, //если запросы идут долго, может увеличиться количество пользователей для поддержания запрсов в секунду
        },
    },
};

export function setup() {
    const response = http.post(
        `${BASE_URL}/api/auth/login`,
        JSON.stringify({
            email: 'dmitry9678@mymail.com',
            password: '5376899317',
        }),
        {
            headers: {
                'Content-Type': 'application/json',
            },
        }
    );

    console.log(`LOGIN STATUS: ${response.status}`);

    return {
        authCookie: response.cookies.auth_cookie[0].value,
    };
}
export default function (data) {
    const response = http.get(
        `${BASE_URL}/api/letter/inbox/0/40`,
        {
            cookies: {
                auth_cookie: data.authCookie,
            },
        }
    );
}