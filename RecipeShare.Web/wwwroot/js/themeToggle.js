// 🌒 Initialize immediately with a default theme if none is set
(function () {
    const root = document.documentElement;
    if (!root.getAttribute("data-theme")) {
        root.setAttribute("data-theme", "dark"); // 👁️ Set dark as the initial theme
    }
})();

// 🌟 Declare the VoidGlass Theme API
window.voidGlassTheme = {
    // 🚀 init: Applies saved theme from localStorage (or defaults to 'dark')
    init: function () {
        const savedTheme = localStorage.getItem("vg-theme") || "dark";
        const root = document.documentElement;
        root.setAttribute("data-theme", savedTheme);
    },

    // 🌗 toggle: Switches between dark and light themes, updates localStorage
    toggle: function () {
        const root = document.documentElement;
        const current = root.getAttribute("data-theme") || "dark";
        const next = current === "dark" ? "light" : "dark";
        root.setAttribute("data-theme", next);
        localStorage.setItem("vg-theme", next);
    },

    // 🛡️ defend: Observes the <html> element and restores `data-theme` if it's ever removed (thanks Blazor 😑)
    defend: function () {
        const observer = new MutationObserver(() => {
            const current = document.documentElement.getAttribute("data-theme");
            if (!current) {
                const fallback = localStorage.getItem("vg-theme") || "dark";
                document.documentElement.setAttribute("data-theme", fallback);
            }
        });

        // 💡 Watch only for attribute changes on <html>
        observer.observe(document.documentElement, {
            attributes: true,
            attributeFilter: ["data-theme"]
        });
    }
};

// 🧠 On page load: initialize and defend the theme from render gremlins
window.addEventListener("load", () => {
    window.voidGlassTheme.init();
    window.voidGlassTheme.defend();
});
