window.scrollIntoViewIfNeeded = (element) => {
    if (!element) return;

    setTimeout(() => {
        element.scrollIntoView({ behavior: "smooth", block: "center" });
    }, 300); // Delay to wait for keyboard to appear
};