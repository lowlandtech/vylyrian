window.scrollInputIntoView = (element) => {
    if (!element) return;

    // Wait until keyboard is visible
    setTimeout(() => {
        element.scrollIntoView({ behavior: "smooth", block: "center" });
    }, 300);
};