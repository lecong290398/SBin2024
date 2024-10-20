// OPP
const app = {
    titlePageChange() {
        // Handle title page change
        var titles = [
            "This is the SBin website!",
            "Welcome to our website!",
            "Check out our latest offers!",
            "Don't miss our coupons!",
            "Scan QR to get more coin!",
            "Contact us for more info!",
            "Follow us via fb fanpage!",
        ];
        let currentTitleIndex = 0;

        var updateTitle = () => {
            document.title = titles[currentTitleIndex];
            currentTitleIndex = (currentTitleIndex + 1) % titles.length;
        }

        setInterval(updateTitle, 5000);
    },
    preLoader() {
        // Handle preload
        document.documentElement.style.overflow = 'hidden';
        const preLoadingPage = document.getElementById('preload')
        window.addEventListener('load', () => {
            preLoadingPage.style.opacity = 0;
            preLoadingPage.style.visibility = 'hidden';
            document.documentElement.style.overflow = 'auto';
            setTimeout(() => {
                preLoadingPage.remove()
            }, 1000)
        });

    },
    start() {
        this.titlePageChange()
        this.preLoader()
    }
}
app.start()


var lastScrollTop = 0;
header = document.getElementById("header");
window.addEventListener("scroll", function () {
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    if (scrollTop > lastScrollTop) {
        header.style.top = "-101px";
    }
    else {
        header.style.top = "0";
    }
    lastScrollTop = scrollTop;
})
var swiper = new Swiper(".mySwiper", {
    slidesPerView: 4,
    spaceBetween: 30,

    scrollbar: {
        el: '.swiper-scrollbar',
        clickable: true,

    },
    breakpoints: {
        320: {
            slidesPerView: 2,
            spaceBetween: 10,
        },
        739: {
            slidesPerView: 2,
            spaceBetween: 10,
        },
        1023: {
            slidesPerView: 3,
            spaceBetween: 20,
        },
        1239: {
            slidesPerView: 4,
            spaceBetween: 30,
        }
    }
});