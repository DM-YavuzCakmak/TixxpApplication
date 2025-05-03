window.api = {
    post: async function (url, data, successCallback, errorCallback) {
        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });

            const contentType = response.headers.get("content-type");

            if (contentType && contentType.includes("application/json")) {
                const result = await response.json();
                if (result.redirectUrl) {
                    window.location.href = result.redirectUrl;
                    return;
                }
                if (result.isSuccess) {
                    showToast("success", result.message || "İşlem başarılı");
                    if (successCallback) successCallback(result);
                } else {
                    showToast("error", result.message || "İşlem başarısız");
                    if (errorCallback) errorCallback(result);
                }
            } else {
                const text = await response.text();
                console.error("Beklenmeyen yanıt:", text);
                showToast("error", "Beklenmeyen sunucu hatası oluştu.");
            }
        } catch (err) {
            console.error("POST error:", err);
            showToast("error", "Bağlantı sırasında bir hata oluştu.");
        }
    }
};
