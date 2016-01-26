(function () {
    var app = angular.module("myApp");
    app.controller('myCtrl', ['carSvc', '$uibModal', function (carSvc, $uibModal) {
        var scope = this;
        scope.years = [];
        scope.makes = [];
        scope.models = [];
        scope.trims = [];
        scope.selected = {
            year: '',
            make: '',
            model: '',
            trim: ''
        }
        scope.cars = [];
        scope.car = '';

        scope.getYears = function () {
            //scope.years = ['1999', '2000', '2001', '2002', '2003'];
            carSvc.getYears().then(function (data) {
                scope.years = data;
                scope.makes = [];
                scope.models = [];
                scope.trims = [];
                scope.selected.years = '';
                scope.selected.make = '';
                scope.selected.model = '';
                scope.selected.trim = '';
                
            });
        }
        scope.getCars = function () {
            carSvc.getCars(scope.selected).then(function (data) {
                scope.cars = data;
            })
        }
        
        scope.getMakes = function () {
            carSvc.getMakes(scope.selected).then(function (data) {
                scope.makes = data;
                scope.models = [];
                scope.trims = [];
                scope.selected.make = '';
                scope.selected.model = '';
                scope.selected.trim = '';
                scope.getCars();
            })
        }
        scope.getModels = function () {
            carSvc.getModels(scope.selected).then(function (data) {
                scope.models = data;
                scope.trims = [];
                scope.selected.model = '';
                scope.selected.trim = '';
                scope.getCars();
            })
        }
        scope.getTrims = function () {
            carSvc.getTrims(scope.selected).then(function (data) {
                scope.trims = data;
                scope.selected.trim = '';
                scope.getCars();
            })
        }
        scope.open = function (id) {
            console.log("Id in open " + id)
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'carModal.html',
                controller: 'carModalCtrl as cm',
                size: 'lg',
                resolve: {
                    car: function () {
                        return carSvc.getDetails(id)
                    }
                }

            });scope.car = modalInstance.car;};
        
            scope.getYears();
    }]);

    
    //app.controller('AccordionDemoCtrl', function AccordionDemoCtrl(scope) {

    //    scope.groups = [
    //      {
    //          title: "Header",
    //          content: "Dynamic Group Body - 1",
    //          open: false
    //      }
    //    ];

    //    scope.addNew = function () {
    //        scope.groups.push({
    //            title: "New One Created",
    //            content: "Dynamically added new one",
    //            open: false
    //        });
    //    }

    //}
  
    app.controller('carModalCtrl', ['$uibModalInstance', 'car', function ($uibModalInstance, car) { // add car later to params

        var scope = this;
        scope.n = 0;
        scope.car = car;
        

        scope.ok = function () {
            $uibModalInstance.close();
        };

        scope.cancel = function () {
            $uibModalInstance.dismiss();
        };
    }])
})();
