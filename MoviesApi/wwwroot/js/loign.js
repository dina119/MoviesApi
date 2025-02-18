document.getElementById("submit_button").onclick = async function (event) {
    event.preventDefault();  // منع إعادة تحميل الصفحة

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    try {
        const response = await fetch("https://localhost:7201/api/Account/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include", // ⬅️ مهم جدًا لتفعيل `HttpOnly Cookies`
            body: JSON.stringify({ userName: username, password: password })
        });

        if (response.ok) {
            document.getElementById("message").innerHTML = "Login Successful!";
            window.location.href = "index.html"; // ✅ توجيه المستخدم إلى الصفحة الرئيسية
        } else {
            document.getElementById("message").innerHTML = "Invalid username or password";
        }
    } catch (error) {
        console.error("Error:", error);
        document.getElementById("message").innerHTML = "Something went wrong!";
    }
};
