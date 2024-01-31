//Ripple Effect for buttons
function createRipple(event) {
    const button = event.currentTarget;

    const circle = document.createElement("span");
    const diameter = Math.max(button.clientWidth, button.clientHeight);
    const radius = diameter / 2;

    circle.style.width = circle.style.height = `${40}px`;
    circle.style.left = `${(diameter / 2) - 20}px`;
    circle.style.top = `${0}px`;
    circle.classList.add("ripple");

    button.appendChild(circle);

    setTimeout(() => {
        circle.remove();
    }, 600);
}

const buttons = document.getElementsByTagName("button");
for (const button of buttons) {
    button.addEventListener("click", createRipple);
}