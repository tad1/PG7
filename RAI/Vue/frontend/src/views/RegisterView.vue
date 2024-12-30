<script setup lang="ts">
    import { ref } from 'vue'
    import { useRouter } from 'vue-router'

    const router = useRouter()

    const username = ref('')
    const password = ref('')
    const confirmPassword = ref('')

    const Register = async () => {
        if(!username.value || !password.value || !confirmPassword.value) {
            return alert('Please fill in all fields')
        }
        if(password.value !== confirmPassword.value) {
            return alert('Passwords do not match')
        }

        await fetch('http://localhost:3000/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username.value,
                password: password.value
            })
        }).then(res => {
            if(!res.ok) {
                alert(res.statusText)
            } else{
                router.push('/login')
            }   
        });
    }

    const vFocus = {
        mounted: (el: HTMLElement) => {
            el.focus()
        }
    }
</script>

<template>
    <main>
        <header>
            <h2>Register</h2>
        </header>
        <form @submit.prevent="Register">
            <table>
                <tbody>
                    <tr>
                        <td><label for="username">Username</label></td>
                        <td><input type="text" id="username" v-model="username" placeholder="username" v-focus></td>
                    </tr>
                    <tr>
                        <td><label for="password">Password</label></td>
                        <td><input type="password" id="password" v-model="password" placeholder="*********"></td>
                    </tr>
                    <tr>
                        <td><label for="confirmPassword">Confirm Password</label></td>
                        <td><input type="password" id="confirm-password" v-model="confirmPassword" placeholder="*********"></td>
                    </tr>
                </tbody>

            </table>
            <input type="submit" value="Register">
        </form>
        <footer>
            <p>Already have an account? <RouterLink to="/login">Login</RouterLink></p>
        </footer>
    </main>
</template>

<style scoped>
/* General styling for the page */
main {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100vh;
    padding: 2rem;
}

header {
    padding: 1.5rem;
    text-align: center;
    background-color: #181818;
    width: 100%;
    color: #fff;
    border-radius: 8px 8px 0 0;
}

footer {
    color: #222;
    text-align: center;
    padding: 1.5rem;
    width: 100%;
    border-radius: 0 0 8px 8px;
}

h2 {
    margin: 0;
    font-size: 2rem;
    color: #fff;
}

form {
    background-color: #fafafa;
    border-radius: 8px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    padding: 2rem;
    width: 100%;
    max-width: 400px;
}

table {
    width: 100%;
    margin-bottom: 1.5rem;
    border-collapse: collapse;
}

td {
    padding: 0.75rem;
}

label {
    font-size: 1rem;
    font-weight: bold;
    color: #333;
    display: block;
}

input[type="text"],
input[type="password"] {
    width: 100%;
    padding: 0.75rem;
    margin-top: 0.25rem;
    border: 1px solid #ccc;
    border-radius: 8px;
    font-size: 1rem;
    box-sizing: border-box;
    color: black;
}

input[type="submit"] {
    background-color: #181818;
    color: #fff;
    font-size: 1.2rem;
    padding: 0.75rem;
    width: 100%;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    margin-top: 1rem;
    transition: background-color 0.3s ease;
}

input[type="submit"]:hover {
    background-color: #333;
}

/* Style for RouterLink in the footer */
footer p {
    font-size: 1rem;
    color: #555;
}

footer p a {
    color: #5a5;
    text-decoration: none;
    font-weight: bold;
}

footer p a:hover {
    color: #3a3;
}

/* Mobile responsiveness */
@media (max-width: 600px) {
    form {
        padding: 1.5rem;
        width: 90%;
    }
}
</style>