window.closeNavMenu = function () {
    const menu = document.getElementById("menu-toggle");

    if (menu) {
        menu.checked = false;
    }
};

window.registerOutsideClick = (profileMenu) => {

    if (!profileMenu) {
        return;
    }

    document.addEventListener("click", function (event) {
        if (!profileMenu) {
            return;
        }
        const clickedInside =
            profileMenu.contains(event.target);
        if (!clickedInside && profileMenu.open) {
            profileMenu.removeAttribute("open");
        }
    });
};

window.registerSidebarOutsideClick = (sidebar, menuToggle) => {

    document.addEventListener("click", function (event) {
        const clickedInsideSidebar = sidebar.contains(event.target);
        const clickedToggle = menuToggle.contains(event.target);
        if (!clickedInsideSidebar && !clickedToggle) {
            if (menuToggle.checked) {
                menuToggle.checked = false;
            }
        }
    });
};