function redirectWithCulture(path) {
    const culture = new URLSearchParams(window.location.search).get("culture") || "tr-TR";
    window.location.href = `${path}${path.includes('?') ? '&' : '?'}culture=${culture}&ui-culture=${culture}`;
}