let toggles = document.getElementsByClassName('toggle');
let contentDiv = document.getElementsByClassName('content');
let icons = document.getElementsByClassName('icon');

for (let i = 0; i < toggles.length; i++) {
    toggles[i].addEventListener('click', () => {
        toggleContent(i);
    });
}

function toggleContent(index) {
    const currentContent = contentDiv[index];
    const currentToggle = toggles[index];
    const currentIcon = icons[index];
    const isExpanded = parseInt(currentContent.style.height) !== currentContent.scrollHeight;

    // Đóng tất cả các nội dung trừ nội dung hiện tại
    for (let j = 0; j < contentDiv.length; j++) {
        if (j !== index) {
            contentDiv[j].style.height = "0px";
            toggles[j].style.color = "var(--black-color)";
            icons[j].classList.remove('fa-minus');
            icons[j].classList.add('fa-plus');
        }
    }

    // Mở hoặc đóng nội dung hiện tại
    if (isExpanded) {
        currentContent.style.height = currentContent.scrollHeight + "px";
        currentToggle.style.color = "var(--dark-green)";
        currentIcon.classList.remove('fa-plus');
        currentIcon.classList.add('fa-minus');
    } else {
        currentContent.style.height = "0px";
        currentToggle.style.color = "var(--black-color)";
        currentIcon.classList.remove('fa-minus');
        currentIcon.classList.add('fa-plus');
    }
}

const myImgModal = document.querySelector('.js-ModalImg')
const myImgModalOverlay = document.querySelector('.js-ModalImg .modal__body')
const imgModal = document.querySelector('.js-img')

function openImgModal(content) {
    const img = content.querySelector('img')
    imgModal.src = img.src
    imgModal.style.borderRadius = "20px"
    myImgModal.classList.add("open")
    document.body.style.overflow = "hidden"
}
function closeImgModal() {
    myImgModal.classList.remove("open")
    document.body.style.overflow = "auto"
}
const zoomUp = document.querySelector('.zoom-up')
const zoomDown = document.querySelector('.zoom-down')
const zoomTranLeft = document.querySelector('.zoom-tran-left')
const zoomTranRight = document.querySelector('.zoom-tran-right')

let scale = 1;
let translateX = 0

zoomUp.onclick = () => {
    scale += 0.2;
    imgModal.style.transform = `scale(${scale}) translateX(${translateX}px)`;
};
zoomDown.onclick = () => {
    scale -= 0.2;
    scale = Math.max(0.2, scale); // Ensure scale doesn't go below a minimum value
    imgModal.style.transform = `scale(${scale}) translateX(${translateX}px)`;
};
zoomTranLeft.onclick = () => {
    translateX += 20; // Translate left by 20px
    imgModal.style.transform = `scale(${scale}) translateX(${translateX}px)`;
};

zoomTranRight.onclick = () => {
    translateX -= 20; // Translate right by 20px
    imgModal.style.transform = `scale(${scale}) translateX(${translateX}px)`;
};