(function () {
  "use strict";

  /* page loader */
  
  function hideLoader() {
    const loader = document.getElementById("loader");
    loader.classList.add("d-none")
  }

  window.addEventListener("load", hideLoader);
  /* page loader */

  /* tooltip */
  const tooltipTriggerList = document.querySelectorAll(
    '[data-bs-toggle="tooltip"]'
  );
  const tooltipList = [...tooltipTriggerList].map(
    (tooltipTriggerEl) => new bootstrap.Tooltip(tooltipTriggerEl)
  );

  /* popover  */
  const popoverTriggerList = document.querySelectorAll(
    '[data-bs-toggle="popover"]'
  );
  const popoverList = [...popoverTriggerList].map(
    (popoverTriggerEl) => new bootstrap.Popover(popoverTriggerEl)
  );

  //switcher color pickers
  const pickrContainerPrimary = document.querySelector(
    ".pickr-container-primary"
  );
  const themeContainerPrimary = document.querySelector(
    ".theme-container-primary"
  );
  const pickrContainerBackground = document.querySelector(
    ".pickr-container-background"
  );
  const themeContainerBackground = document.querySelector(
    ".theme-container-background"
  );

  /* for theme primary */
  if (window.Pickr && pickrContainerPrimary && themeContainerPrimary) {
    const nanoThemes = [
      [
        "nano",
        {
          defaultRepresentation: "RGB",
          components: {
            preview: true,
            opacity: false,
            hue: true,

            interaction: {
              hex: false,
              rgba: true,
              hsva: false,
              input: true,
              clear: false,
              save: false,
            },
          },
        },
      ],
    ];
    const nanoButtons = [];
    let nanoPickr = null;
    for (const [theme, config] of nanoThemes) {
      const button = document.createElement("button");
      button.innerHTML = theme;
      nanoButtons.push(button);

      button.addEventListener("click", () => {
        const el = document.createElement("p");
        pickrContainerPrimary.appendChild(el);

        /* Delete previous instance */
        if (nanoPickr) {
          nanoPickr.destroyAndRemove();
        }

        /* Apply active class */
        for (const btn of nanoButtons) {
          btn.classList[btn === button ? "add" : "remove"]("active");
        }

        /* Create fresh instance */
        nanoPickr = new Pickr(
          Object.assign(
            {
              el,
              theme,
              default: "#845adf",
            },
            config
          )
        );

        /* Set events */
        nanoPickr.on("changestop", (source, instance) => {
          let color = instance.getColor().toRGBA();
          let html = document.querySelector("html");
          html.style.setProperty(
            "--primary-rgb",
            `${Math.floor(color[0])}, ${Math.floor(color[1])}, ${Math.floor(
              color[2]
            )}`
          );
          /* theme color picker */
          localStorage.setItem(
            "primaryRGB",
            `${Math.floor(color[0])}, ${Math.floor(color[1])}, ${Math.floor(
              color[2]
            )}`
          );
          updateColors();
        });
      });

      themeContainerPrimary.appendChild(button);
    }
    nanoButtons[0].click();
  }
  /* for theme primary */

  /* for theme background */
  if (window.Pickr && pickrContainerBackground && themeContainerBackground) {
    const nanoThemes1 = [
      [
        "nano",
        {
          defaultRepresentation: "RGB",
          components: {
            preview: true,
            opacity: false,
            hue: true,

            interaction: {
              hex: false,
              rgba: true,
              hsva: false,
              input: true,
              clear: false,
              save: false,
            },
          },
        },
      ],
    ];
    const nanoButtons1 = [];
    let nanoPickr1 = null;
    for (const [theme, config] of nanoThemes1) {
      const button = document.createElement("button");
      button.innerHTML = theme;
      nanoButtons1.push(button);

      button.addEventListener("click", () => {
        const el = document.createElement("p");
        pickrContainerBackground.appendChild(el);

        /* Delete previous instance */
        if (nanoPickr1) {
          nanoPickr1.destroyAndRemove();
        }

        /* Apply active class */
        for (const btn of nanoButtons1) {
          btn.classList[btn === button ? "add" : "remove"]("active");
        }

        /* Create fresh instance */
        nanoPickr1 = new Pickr(
          Object.assign(
            {
              el,
              theme,
              default: "#845adf",
            },
            config
          )
        );

        /* Set events */
        nanoPickr1.on("changestop", (source, instance) => {
          let color = instance.getColor().toRGBA();
          let html = document.querySelector("html");
          html.style.setProperty(
            "--body-bg-rgb",
            `${color[0]}, ${color[1]}, ${color[2]}`
          );
          document
            .querySelector("html")
            .style.setProperty(
              "--body-bg-rgb2",
              `${color[0] + 14}, ${color[1] + 14}, ${color[2] + 14}`
            );
          document
            .querySelector("html")
            .style.setProperty(
              "--light-rgb",
              `${color[0] + 14}, ${color[1] + 14}, ${color[2] + 14}`
            );
          document
            .querySelector("html")
            .style.setProperty(
              "--form-control-bg",
              `rgb(${color[0] + 14}, ${color[1] + 14}, ${color[2] + 14})`
            );
          localStorage.removeItem("bgtheme");
          updateColors();
          html.setAttribute("data-theme-mode", "dark");
          html.setAttribute("data-menu-styles", "dark");
          html.setAttribute("data-header-styles", "dark");
          const d = document.querySelector("#switcher-dark-theme");
          if (d) d.checked = true;
          localStorage.setItem(
            "bodyBgRGB",
            `${color[0]}, ${color[1]}, ${color[2]}`
          );
          localStorage.setItem(
            "bodylightRGB",
            `${color[0] + 14}, ${color[1] + 14}, ${color[2] + 14}`
          );
        });
      });
      themeContainerBackground.appendChild(button);
    }
    nanoButtons1[0].click();
  }
  /* for theme background */

  /* header theme toggle */
  function toggleTheme() {
    let html = document.querySelector("html");
    if (html.getAttribute("data-theme-mode") === "dark") {
      html.setAttribute("data-theme-mode", "light");
      html.setAttribute("data-header-styles", "light");
      html.setAttribute("data-menu-styles", "light");
      if (!localStorage.getItem("primaryRGB")) {
        html.setAttribute("style", "");
      }
      html.removeAttribute("data-bg-theme");
      const slt = document.querySelector("#switcher-light-theme");
      const sml = document.querySelector("#switcher-menu-light");
      if (slt) slt.checked = true;
      if (sml) sml.checked = true;
      document
        .querySelector("html")
        .style.removeProperty("--body-bg-rgb", localStorage.bodyBgRGB);
      checkOptions();
      html.style.removeProperty("--body-bg-rgb2");
      html.style.removeProperty("--light-rgb");
      html.style.removeProperty("--form-control-bg");
      html.style.removeProperty("--input-border");
      const shl = document.querySelector("#switcher-header-light");
      const sml2 = document.querySelector("#switcher-menu-light");
      const slt2 = document.querySelector("#switcher-light-theme");
      const bg4 = document.querySelector("#switcher-background4");
      const bg3 = document.querySelector("#switcher-background3");
      const bg2 = document.querySelector("#switcher-background2");
      const bg1 = document.querySelector("#switcher-background1");
      const bg0 = document.querySelector("#switcher-background");
      if (shl) shl.checked = true;
      if (sml2) sml2.checked = true;
      if (slt2) slt2.checked = true;
      if (bg4) bg4.checked = false;
      if (bg3) bg3.checked = false;
      if (bg2) bg2.checked = false;
      if (bg1) bg1.checked = false;
      if (bg0) bg0.checked = false;
      localStorage.removeItem("Adminordarktheme");
      localStorage.removeItem("AdminorMenu");
      localStorage.removeItem("AdminorHeader");
      localStorage.removeItem("bodylightRGB");
      localStorage.removeItem("bodyBgRGB");
      if (localStorage.getItem("Adminorlayout") != "horizontal") {
        html.setAttribute("data-menu-styles", "light");
      }
      html.setAttribute("data-header-styles", "gradient");
    } else {
      html.setAttribute("data-theme-mode", "dark");
      html.setAttribute("data-header-styles", "gradient");
      if (!localStorage.getItem("primaryRGB")) {
        html.setAttribute("style", "");
      }
      html.setAttribute("data-menu-styles", "dark");
      const sdt = document.querySelector("#switcher-dark-theme");
      const smd = document.querySelector("#switcher-menu-dark");
      const shd = document.querySelector("#switcher-header-dark");
      if (sdt) sdt.checked = true;
      if (smd) smd.checked = true;
      if (shd) shd.checked = true;
      checkOptions();
      if (smd) smd.checked = true;
      if (shd) shd.checked = true;
      if (sdt) sdt.checked = true;
      const bg4 = document.querySelector("#switcher-background4");
      const bg3 = document.querySelector("#switcher-background3");
      const bg2 = document.querySelector("#switcher-background2");
      const bg1 = document.querySelector("#switcher-background1");
      const bg0 = document.querySelector("#switcher-background");
      if (bg4) bg4.checked = false;
      if (bg3) bg3.checked = false;
      if (bg2) bg2.checked = false;
      if (bg1) bg1.checked = false;
      if (bg0) bg0.checked = false;
      localStorage.setItem("Adminordarktheme", "true");
      localStorage.setItem("AdminorMenu", "dark");
      localStorage.setItem("AdminorHeader", "dark");
      localStorage.removeItem("bodylightRGB");
      localStorage.removeItem("bodyBgRGB");
    }
  }
  let layoutSetting = document.querySelector(".layout-setting");
  if (layoutSetting) layoutSetting.addEventListener("click", toggleTheme);
  /* header theme toggle */

  /* Choices JS */
  document.addEventListener("DOMContentLoaded", function () {
    var genericExamples = document.querySelectorAll("[data-trigger]");
    for (let i = 0; i < genericExamples.length; ++i) {
      var element = genericExamples[i];
      new Choices(element, {
        allowHTML: true,
        placeholderValue: "This is a placeholder set in the config",
        searchPlaceholderValue: "Search",
      });
    }
  });
  /* Choices JS */

  /* footer year */
  document.getElementById("year").innerHTML = new Date().getFullYear();
  /* footer year */

  /* node waves */
  Waves.attach(".btn-wave", ["waves-light"]);
  Waves.init();
  /* node waves */

  /* card with close button */
  let DIV_CARD = ".card";
  let cardRemoveBtn = document.querySelectorAll(
    '[data-bs-toggle="card-remove"]'
  );
  cardRemoveBtn.forEach((ele) => {
    ele.addEventListener("click", function (e) {
      e.preventDefault();
      let $this = this;
      let card = $this.closest(DIV_CARD);
      card.remove();
      return false;
    });
  });
  /* card with close button */

  /* card with fullscreen */
  let cardFullscreenBtn = document.querySelectorAll(
    '[data-bs-toggle="card-fullscreen"]'
  );
  cardFullscreenBtn.forEach((ele) => {
    ele.addEventListener("click", function (e) {
      let $this = this;
      let card = $this.closest(DIV_CARD);
      card.classList.toggle("card-fullscreen");
      card.classList.remove("card-collapsed");
      e.preventDefault();
      return false;
    });
  });
  /* card with fullscreen */

  /* count-up */
  var i = 1;
  setInterval(() => {
    document.querySelectorAll(".count-up").forEach((ele) => {
      if (ele.getAttribute("data-count") >= i) {
        i = i + 1;
        ele.innerText = i;
      }
    });
  }, 10);
  /* count-up */

  /* back to top */
  const scrollToTop = document.querySelector(".scrollToTop");
  const $rootElement = document.documentElement;
  const $body = document.body;
  window.onscroll = () => {
    const scrollTop = window.scrollY || window.pageYOffset;
    const clientHt = $rootElement.scrollHeight - $rootElement.clientHeight;
    if (window.scrollY > 100) {
      scrollToTop.style.display = "flex";
    } else {
      scrollToTop.style.display = "none";
    }
  };
  scrollToTop.onclick = () => {
    window.scrollTo(0, 0);
  };
  /* back to top */

  /* header dropdowns scroll */
  var myHeaderShortcut = document.getElementById("header-shortcut-scroll");
  new SimpleBar(myHeaderShortcut, { autoHide: true });

  var myHeadernotification = document.getElementById(
    "header-notification-scroll"
  );
  new SimpleBar(myHeadernotification, { autoHide: true });

  var myHeaderCart = document.getElementById("header-cart-items-scroll");
  new SimpleBar(myHeaderCart, { autoHide: true });
  /* header dropdowns scroll */
})();

/* full screen */
var elem = document.documentElement;
function openFullscreen() {
  let open = document.querySelector(".full-screen-open");
  let close = document.querySelector(".full-screen-close");

  if (
    !document.fullscreenElement &&
    !document.webkitFullscreenElement &&
    !document.msFullscreenElement
  ) {
    if (elem.requestFullscreen) {
      elem.requestFullscreen();
    } else if (elem.webkitRequestFullscreen) {
      /* Safari */
      elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) {
      /* IE11 */
      elem.msRequestFullscreen();
    }
    if (close) close.classList.add("d-block");
    if (close) close.classList.remove("d-none");
    if (open) open.classList.add("d-none");
  } else {
    if (document.exitFullscreen) {
      document.exitFullscreen();
    } else if (document.webkitExitFullscreen) {
      /* Safari */
      document.webkitExitFullscreen();
      console.log("working");
    } else if (document.msExitFullscreen) {
      /* IE11 */
      document.msExitFullscreen();
    }
    if (close) close.classList.remove("d-block");
    if (open) open.classList.remove("d-none");
    if (close) close.classList.add("d-none");
    if (open) open.classList.add("d-block");
  }
}
/* full screen */

/* toggle switches */
let customSwitch = document.querySelectorAll(".toggle");
customSwitch.forEach((e) =>
  e.addEventListener("click", () => {
    e.classList.toggle("on");
  })
);
/* toggle switches */

/* header dropdown close button */

/* for cart dropdown */
/* for cart dropdown */
const headerbtn = document.querySelectorAll(".header-cart-remove");

headerbtn.forEach((button) => {
  button.addEventListener("click", (e) => {
    e.preventDefault();
    e.stopPropagation();
    let cartItem = button.closest('.dropdown-item');
    if (cartItem) {
      cartItem.remove();
      document.getElementById("cart-data").innerText = `${
        document.querySelectorAll(".dropdown-item-close").length
      } Items`;
      document.getElementById("cart-icon-badge").innerText = `${
        document.querySelectorAll(".dropdown-item-close").length
      }`;
    }
    if (document.querySelectorAll(".dropdown-item-close").length == 0) {
      let elementHide = document.querySelector(".empty-header-item");
      let elementShow = document.querySelector(".empty-item");
      elementHide.classList.add("d-none");
      elementShow.classList.remove("d-none");
    }
  });
});
/* for cart dropdown */
/* for cart dropdown */

/* for notifications dropdown */
const headerbtn1 = document.querySelectorAll(".dropdown-item-close1");
headerbtn1.forEach((button) => {
  button.addEventListener("click", (e) => {
    e.preventDefault();
    e.stopPropagation();
    button.parentNode.parentNode.parentNode.parentNode.remove();
    document.getElementById("notifiation-data").innerText = `${
      document.querySelectorAll(".dropdown-item-close1").length
    } Unread`;
    // document.getElementById("notification-icon-badge").innerText = `${
    //   document.querySelectorAll(".dropdown-item-close1").length
    // }`;
    if (document.querySelectorAll(".dropdown-item-close1").length == 0) {
      let elementHide1 = document.querySelector(".empty-header-item1");
      let elementShow1 = document.querySelector(".empty-item1");
      elementHide1.classList.add("d-none");
      elementShow1.classList.remove("d-none");
    }
  });
});
/* for notifications dropdown */

/* for message dropdown */
const headerbtn2 = document.querySelectorAll(".dropdown-item-close3");
headerbtn2.forEach((button) => {
  button.addEventListener("click", (e) => {
    e.preventDefault();
    e.stopPropagation();
    button.parentNode.parentNode.parentNode.parentNode.remove();
    document.getElementById("message-data").innerText = `${
      document.querySelectorAll(".dropdown-item-close3").length
    } `;
    // document.getElementById("notification-icon-badge").innerText = `${
    //   document.querySelectorAll(".dropdown-item-close3").length
    // }`;
    if (document.querySelectorAll(".dropdown-item-close3").length == 0) {
      let elementHide2 = document.querySelector(".empty-header-item3");
      let elementShow2 = document.querySelector(".empty-item3");
      elementHide2.classList.add("d-none");
      elementShow2.classList.remove("d-none");
    }
  });
});

/* for Search */
document.addEventListener('DOMContentLoaded', function () {
  // Add click event to the search button
  document.getElementById('searchButton').addEventListener('click', function () {
      // Toggle active class on the navbar-form
      document.getElementById('myNavbarForm').classList.toggle('active');
  });

  // Add click event to the close button
  document.querySelector('.close-btn').addEventListener('click', function () {
      // Remove active class from the navbar-form
      document.getElementById('myNavbarForm').classList.remove('active');
  });
  // Ensure sidebar toggle works regardless of theme script state
  const sidemenuToggle = document.querySelector('.sidemenu-toggle');
  if (sidemenuToggle) {
    sidemenuToggle.addEventListener('click', function () {
      const html = document.documentElement;
      const verticalStyle = html.getAttribute('data-vertical-style') || 'overlay';
      if (verticalStyle === 'overlay') {
        const isClosed = html.getAttribute('data-toggled') === 'icon-overlay-close';
        if (isClosed) {
          html.removeAttribute('data-toggled');
        } else {
          html.setAttribute('data-toggled', 'icon-overlay-close');
        }
      } else {
        const isClosed = html.getAttribute('data-toggled') === 'close';
        if (isClosed) {
          html.removeAttribute('data-toggled');
        } else {
          html.setAttribute('data-toggled', 'close');
        }
      }
    });
  }
});
/* for Search */