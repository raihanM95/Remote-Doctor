var app = angular.module("Hospital", ['ngRoute']);
app.controller("DoctorController", function ($scope,$http) {
    $http.get('/Home/GetData').then(function(d) {
            $scope.Doctor = d.data;
        },
        function(error) {
            alert("Faild");
        });
});

app.controller("DoctorSearch", function ($scope, $http) {
    $http.get('/Patient/Appointment').then(function (d) {
            $scope.Doctor = d.data;
        },
        function (error) {
            alert("Faild");
        });
});


