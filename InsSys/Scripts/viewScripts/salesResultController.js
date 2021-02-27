var app = new Vue({
    el: '#salesPresentation',
    data: function () {
        return {
            labelsThisYear: [],
            valuesThisYear: [],
            labelsYearBefore: [],
            valuesYearBefore: [],
        };
    },
    created: function() {
        this.getSalesSixMonthsBack();
        this.getSalesSixMonthsYearBefore();

    },
    methods: {
        CreateChart: function (id, labels, dataset, label, color) {
            let myCh = document.getElementById(id).getContext('2d');

            let lastSalesChart = new Chart(myCh, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: label,
                        data: dataset, backgroundColor: color,
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        },
        async getSalesSixMonthsBack() {
            var res = await fetch('/SalesResult/GetSalesSixMonthsBack');
            var json = await res.json();
            let labels = JSON.parse(json).map(a => a.Label);
            let values = JSON.parse(json).map(a => a.Sales);
            this.CreateChart('SalesChart2', labels, values, 'Sales', 'rgba(51,122,183, 1)');
        },
        async getSalesSixMonthsYearBefore() {
            var res = await fetch('/SalesResult/GetSalesSixMonthsYearBefore');
            var json = await res.json();
            let labels = JSON.parse(json).map(a => a.Label);
            let values = JSON.parse(json).map(a => a.Sales);
            this.CreateChart('SalesChart', labels, values, 'Sales - year before', 'rgba(51,122,183, .8)');
        },

    }
});
