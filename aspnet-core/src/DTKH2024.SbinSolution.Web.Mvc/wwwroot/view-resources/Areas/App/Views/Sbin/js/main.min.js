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
        const preLoadingPage = document.getElementById('preload')
        window.addEventListener('load', () => {
            preLoadingPage.style.opacity = 0;
            preLoadingPage.style.visibility = 'hidden';
            setTimeout(() => {
                preLoadingPage.remove()
            }, 1000)
        });

    },
    blinkUserImg() {
        const userImg = document.querySelector('.user__profile');

        if (userImg) {
            let isVisible = false;

            userImg.onclick = () => {
                const showMore = userImg.querySelector('.user__show-more');
                if (showMore) {
                    if (isVisible) {
                        showMore.style.visibility = 'hidden';
                        showMore.style.transform = 'scale(0)';
                        showMore.style.opacity = '0';
                    } else {
                        showMore.style.visibility = 'visible';
                        showMore.style.transform = 'scale(1)';
                        showMore.style.opacity = '1';
                    }
                    isVisible = !isVisible;  // Toggle the state
                } else {
                    console.log('Show more element not found');
                }
            };
        } else {
            console.log('User profile image not found');
        }

    },
    onrender() {
    },
    start() {
        this.titlePageChange()
        this.preLoader()
        this.blinkUserImg()
        // this.onrender()
    }
}
app.start()