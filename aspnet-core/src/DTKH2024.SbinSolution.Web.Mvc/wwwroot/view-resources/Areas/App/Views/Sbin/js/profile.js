const ctx = document.getElementById('myChart');
const randomNum = Array.from({ length: 12 }, () => Math.floor(Math.random() * 100));
const totalElement = document.getElementById('total-earned'); 

var total = randomNum.reduce((a, b) => a + b); 
totalElement.innerText = total;

new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        datasets: [{
            label: 'Number of coins',
            data: randomNum,
            borderWidth: 1,
            borderColor: '#20914f',
            backgroundColor: '#2ecc71',
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});
