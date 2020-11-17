var app = new Vue({
    el: '#Element',
    data: function () {
        return {
            insurances: null,
            insurancesVisible: null,
            res: '',
            disabledRetract: true,
            disabledAdd: true,
            pageNo: 1,
            elementsOnPage: 7,
        };
    },
    created: function() {
        this.getInsurances();
    },
    methods: {
        request: function (method, url) {
            
        },
        async getInsurances() {
            var res = await fetch('/ManageInsurances/GetInsurances');
            var json = await res.json();
            this.insurances = JSON.parse(json);
            this.showPolicies();
            
        },
        showPolicies: function () {
            this.insurancesVisible = this.insurances.slice((this.pageNo - 1) * this.elementsOnPage, this.pageNo * this.elementsOnPage);
            if (this.insurances.length - this.pageNo * this.elementsOnPage >= 0) {
                console.log("disabledAdd=false");
                this.disabledAdd = false;
            }
            if (this.pageNo != 1) {
                this.disabledRetract = false;
            }
        },
        nextPage: function () {
            var dif = this.insurances.length - this.pageNo * this.elementsOnPage;
            this.disabledRetract = false;
            this.pageNo = this.pageNo + 1;
            if (dif < this.elementsOnPage) {
                this.disabledAdd = true;
            }
            this.insurancesVisible = this.insurances.slice((this.pageNo - 1) * this.elementsOnPage, this.pageNo * this.elementsOnPage);
        },
        previousPage: function () {
            this.disabledAdd = false;
            this.pageNo = this.pageNo - 1;
            if (this.pageNo == 1) {
                this.disabledRetract = true;
            }
            this.insurancesVisible = this.insurances.slice((this.pageNo - 1) * this.elementsOnPage, this.pageNo * this.elementsOnPage);
        },
        async generatePolicy(id) {
            let res = await fetch('/ManageInsurances/GeneratePolicyDocument?id=' + id, { method: 'post' });
            console.log(res);
        },
        async deletePolicy(id) {
            let res = await fetch('/ManageInsurances/DeletePolicy?id=' + id, { method: 'post' });
            let status = res.status;
            if (status == 200) {
                alert("Deleted!");
                location.reload(true);
            }
        },
        async showPolicy(event, id) {
            let coordinates = event.target.getBoundingClientRect();
            let res = await fetch('/ManageInsurances/ShowPolicy?id=' + id, { method: 'get' });
            let result = await res.text();
            console.log(result);
            $("#policymodal").empty();
            $("#policymodal").html(result);
            $('#modal').css("top", coordinates.top + 15 + "px");
            $('#modal').css("right", window.innerWidth - coordinates.right + 15 + "px");
            return false;
        }
    }
});
