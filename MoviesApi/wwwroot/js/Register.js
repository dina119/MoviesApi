document.getElementById("submit_button").onclick = async function (event) {
    event.preventDefault();  // منع إعادة تحميل الصفحة

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const email = document.getElementById("email").value;
    const confirmPassword = document.getElementById("confirmPassword").value;
    try {
        const response = await fetch("https://localhost:7201/api/Account/Register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include", // ⬅️ مهم جدًا لتفعيل `HttpOnly Cookies`
            body: JSON.stringify({ userName: username, email: email, password: password,  confirmPassword: confirmPassword })
        });

        if (response.ok) {
            document.getElementById("message").innerHTML = "SignUp  Successful!";
            window.location.href = "index.html"; // ✅ توجيه المستخدم إلى الصفحة الرئيسية
        } else {
            let data = await response.json();
            errorMessage.textContent = data.message || "Registration failed!";
        }
    } catch (error) {
        console.error("Error:", error);
        document.getElementById("message").innerHTML = "Something went wrong!";
    }
};
