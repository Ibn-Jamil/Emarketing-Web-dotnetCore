var shareApp = angular.module("sharedApp", []);
shareApp.controller("sharedController", function ($scope, $rootScope, $http) {

    $http.get("/Api/NavbarApi").then(function (response) {
        $scope.Nabardata = response.data
    }, function () {
        $scope.Nabardata = "Categories not loaded Properly";
    });
    $scope.serachFromNavbar = function (id, genre) {
        var str = window.location.toString();
        if (str.search("Home/Index") == -1 && window.location.pathname.localeCompare("/")!=0)
            window.location.href = "/Home/Index";
        $rootScope.$emit("FilterDataFormNavbar", {Id:id,Genre:genre});
        
    };
    $scope.disableBubbling = function (id, e) {
        e = e || window.event;
        var elem = $("#catSub" + id);
        var currentstatus = elem.css('display');
        var currentstatusChild = elem.children().css('display');
        $(".ChildList-more").hide();   
        $(".grandChildList").hide();
        if (currentstatus == 'none') {
            elem.show();
            if (currentstatusChild == 'none')
            currentstatusChild.show();
        }
        e.stopPropagation();
    }
    $scope.hideOpenedCatagories = function () {
        $(".ChildList-more").hide();
    };
    $scope.nestedDD = function (id, e) {
        e = e || window.event;
        var elem = $("#cat" + id);
       var currentstatus= elem.css('display');
        $(".grandChildList").hide();
        if (currentstatus == 'none')
        elem.toggle();
        e.stopPropagation();
    }
    $scope.hideOpenedChildCatagories = function () {
        $(".grandChildList").hide();
    };
})



//****************************************
shareApp.controller("detailController", function ($scope, $http,$sce) {
    var str = window.location.toString();
    var hostName = window.location.pathname;
    var serverPath = str.slice(0, str.indexOf(hostName));
    var n = str.lastIndexOf("/");
    var id = str.slice(n + 1);
    var slideIndex = 1;
    var reviewSum = 0;
    $scope.id = id;
    $scope.serverPath = serverPath;
    $scope.oneStar = 0;
    $scope.twoStar = 0;
    $scope.threeStar = 0;
    $scope.fourStar = 0;
    $scope.fivestar = 0;


    $scope.plusSlides = function (n) {
        slideIndex = showSlides(slideIndex += n, slideIndex);
    };
    $scope.currentSlide = function (n) {
        slideIndex = n + 1
        showSlides(n + 1, slideIndex);
    }
    $http.get("/Api/HomeApi/" + id).then(function (response) {
        $scope.detail = response.data;
        $scope.seller = $scope.detail.productsDetailsDto.productSellerId;
        $scope.detailDescription = $sce.trustAsHtml(response.data.productsDetailsDto.productDescription);
        $http.get("/Api/FilterProductApi/" + $scope.detail.productsDetailsDto.catagoriesSpecificId + "/" + $scope.detail.productsDetailsDto.id)
            .then(function (response) {
                $scope.trendData = response.data;
            }, function () {
                $scope.trendData = "Reviews Not Loaded properly Please Try Again";
            });
        $scope.detail.customersReviewListDto.forEach(function (item) {
            reviewSum += item.ratingStars;
            switch (item.ratingStars) {
                case 1: $scope.oneStar++; break;
                case 2: $scope.twoStar++; break;
                case 3: $scope.threeStar++; break;
                case 4: $scope.fourStar++; break;
                case 5: $scope.fivestar++; break;
            }
        }
        )
        $scope.averageReview = (reviewSum / $scope.detail.customersReviewListDto.length).toPrecision(2);
    }, function () {
        $scope.detail = "details Data Not loaded Properly Please try again";
    });
    $scope.deleteProduct = function (id) {
        $http.delete("/Api/HomeApi/" + id).then(function () {
            document.location.href = "/Home/Index";
        }, function () {
            toastr.error("Delete was unsuccessful");
        });
    };
    $scope.loadAllQuestion = function (id) {
        $http.get("/Api/CustomerApi/" + id).then(function (response) {
            $scope.detail.productsFeedbackListDto = response.data;
        }, function () {
            toastr.error("All Questions not load Please try again");
        })
    }
    $scope.AddToCart = function (id) { AddToCart(id); }
    $scope.formateDate = function (datestr) { return dateFormate(datestr); };
    $scope.LinearRating = function (rating, str) { LinearRating(rating, str); };
    $scope.discountRate = function (rate, discount) { return Math.round(rate * (1 - discount / 100)); }
    $scope.subtractDate = function (Qtime, AnsTime) { return dateSubtract(Qtime, AnsTime); }
    $scope.fillStar = function (x) { fillStar(x); };
    $scope.rateProduct = function (x) { rateProduct(x); };
    $scope.openTextArea = function (index, id, seller) {
        openTextArea(index, id, seller);
    }
    $scope.askQuestion = function (productId, loginUser) {
        askQuestion(productId, loginUser);
    }
    $scope.openTextAreaForReview = function (index, reviewId, seller) {
        openTextAreaForReview(index, reviewId, seller);
    }
});

//*********************************************************
shareApp.controller('homeIndexController', function ($scope, $rootScope, $http) {
    var str = window.location.toString();
    var n = str.lastIndexOf(window.location.pathname);
    var serverPath = str.slice(0, n);
    $scope.serverPath = serverPath;
    $scope.catagoryGenre = 0;
    var priceSequence = 0;
    var catagorySequence = 0;
    loadMajorCatagories();
    localData(0, 0, 0, 1);
    $rootScope.$on("FilterDataFormNavbar", function (event, data) {
        localData(0, data.Id, data.Genre, 1);
    });
    $scope.goHome = function () {
        $scope.catagoryGenre = 0;
        localData(0, 0, 0, 1);
    };
    $scope.changePage = function (pageNumber) {
        localData(0, 0, 0, pageNumber);
    }
    $scope.AddToCart = function (id, quickShop) {
        AddToCart(id, quickShop);
    }
    $scope.filterProduct = function () {
        var trendQuery = "/Api/FilterProduct?Catagory=0&ProductId=0";
        $http.get(serverPath + trendQuery).then(function (response) {
            $scope.trendData = response.data;
        },
            function () {
                $scope.trendData = "error in loading Trend Data";
            });

    };
    $scope.LinearRating = function (rating, str) { LinearRating(rating, str) };
    $scope.discountRate = function (rate, discount) { return Math.round(rate * (1 - discount / 100)); }
    $scope.searchbyQuery = function () {
        var query = $("#typeahead").val();
        $http.get("/Api/FilterProductApi/search?Query=" + query).then(function (response) {
            $scope.data = response.data;
        }, function () { toastr.error("Data Not Loaded Successfully!") }
        );
    }
    $scope.SaleProducts = function (pageIndex) {
        $http.get("/Api/Sales?PageIndex=" + pageIndex).then(function (response) {
            $scope.data = response.data;
        }, function () { toastr.error("Data Not Loaded Successfully!") }
        );
    };
    $scope.sortByPrice = function (price) {
        priceSquence = price;
        localData(priceSquence, catagorySequence, $scope.catagoryGenre, 1);
    };
    $scope.sortByCatagory = function (catSequence) {
        if (catSequence == 0) {
            $scope.catagoryGenre = 0;
            localData(0, 0, 0, 1);
            loadMajorCatagories();
        };
        if (catSequence>0) {
            catagorySequence = catSequence;
            switch ($scope.catagoryGenre) {
                case 0: {
                    $scope.catagorySequenceOpt = null;
                    $http.get(serverPath + "/Api/loadSubCataogryApi?MajorId=" + catagorySequence).then(function (response) {
                        $scope.catagoryGenre = 1;
                        $("#sortByCatagoryPlaceHolder").text("--Category > Sub Category--");
                        $scope.catagorySequenceOpt = response.data;
                        localData(priceSequence, catagorySequence, $scope.catagoryGenre, 1);
                    }, function () { toastr.error("Categories not Load Successfully!") });
                    break;
                }
                case 1: {
                    $scope.catagorySequenceOpt = null;
                    $http.get("/Api/loadSpecficCatagoryApi?MajorSubId=" + catagorySequence).then(function (response) {
                        $scope.catagoryGenre = 2;
                        $("#sortByCatagoryPlaceHolder").text("--Sub Category > Specific--");
                        $scope.catagorySequenceOpt = response.data;
                        localData(priceSequence, catagorySequence, $scope.catagoryGenre, 1)
                    }, function () { toastr.error("Categories not Load Successfully!") });
                    break;
                }
                case 2: {
                    localData(priceSequence, catagorySequence, $scope.catagoryGenre, 1);
                    break;
                }
            }
        }
    }
    function localData(priceSquence, catagorySequence, catagoryGenre, pageIndex) {
        $scope.currentPage = pageIndex;
        $http.get("/Api/HomeApi?SortByPrice=" + priceSquence + "&SortByCatagory=" + catagorySequence +
            "&CatagoryGenre=" + catagoryGenre + "&PageIndex=" + pageIndex).then(function (response) {
            $scope.data = response.data;
            $scope.requestStatus = response.status;
            var length = Math.ceil(response.data[0].productsCounter / 32);
            var pageArray = [];
            for (i = 1; i <= length; i++) {
                pageArray.push(i);
            }
            $scope.pages = pageArray;
        }, function () { toastr.error("data not loaded successfully!") });
    };
    function loadMajorCatagories() {
        $http.get("/Api/loadMajorCatagoriesApi").then(function (response) {
            $("#sortByCatagoryPlaceHolder").text("--Sort by Category--");
            $scope.catagorySequenceOpt = null;
            $scope.catagorySequenceOpt = response.data;
        }, function () { toastr.error("data not loaded successfully!") })
    }
});
shareApp.controller("loadSubCatagoriesController", function ($scope, $http) {
    var selected = $("#CatagoriesMajor_Id");
    selected.change(function () {
        $http.get("/Api/loadSubCataogryApi?MajorId=" + selected.val()).then(function (response) {
            $scope.subCatagoryData = response.data;
        }, function () { toastr.error("Sub Categories not Load ! please try Again") });
    });
});


//-----------------------> Post Product <----------------------------
shareApp.controller("postProductController", function ($scope, $http) {
    $scope.loadChildCatagory = function (id) {
        $http.get("/Api/loadSubCataogryApi?MajorId="+id).then(function (response) {
            $scope.majorSubList = response.data;
        }, function () { toastr.warning("Major Sub Category Not Load Properly") });
    };
    $scope.loadGrandChildCatagory=function (id) {
        $http.get("/Api/loadSpecficCatagoryApi?MajorSubId=" +id).then(function (response) {
            $scope.catSpecficList = response.data;
        }, function () { toastr.warning("Major Sub Category Not Load Properly") });
    };
    var sizesQuantity=[];
    $scope.AddSizeandQuantity = function (size) {
        if (size != null && size !="--No Size--") {
            sizesQuantity.push(size);
            $scope.sizesQuantity = sizesQuantity;
            $("#Sizelist option[value=" + size + "]").remove();
        }
        if (size == "--No Size--") {
            $scope.sizesQuantity = [];
            sizesQuantity = [];
        }
    }
    $scope.AddSizeandQuantityEdit = function (size, count) {
        SizeandQuantityEditHtml(size, count);
        $scope.AddSizeandQuantity(size);
    }
    var ImageCountArr = [];
    $scope.addImage = function () {
        ImageCountArr.push(ImageCountArr.length+1);
        $scope.ImageCountArray = ImageCountArr;
    }
    $scope.removeimg = function (id) {
        $scope.ImageCountArray.splice(id,1);
    }
    $scope.listOfSizes = listOfAllSizes();
    $scope.ChangSize = function(value, index) {
        $("#Prod_Sizes_" + index + "__AllSizes").val(value);
        $("#Prod_Sizes_" + index + "__AllSizes + p").html(value);
    }
    $scope.addSize = function (count) {
        for (var i = 0; i < count; i++) {
            var opt = $("#Prod_Sizes_" + i + "__AllSizes").val();
            var index=$scope.listOfSizes.indexOf(opt);
            $scope.listOfSizes.splice(index,1);
           
        }
        $("#AddsizeButton").hide();
        $("#AddSize").show();
    }
    $scope.ChangeCurrentCatagory = function (value) {
        alert(value);
        $("#Prod_CatagoriesSpecificId").val(value);
    }
});
shareApp.controller("placeOrderController", function ($scope, $http) {
    $scope.step1 = true; $scope.step2 = false;
    $scope.obj = []; $scope.unitPrice = []; $scope.SumCart = 0; $scope.loadQuantity = [];
    $scope.passValueToAngular = function (cost, transportCharges) {
        $scope.obj.push({ price: cost, deliveryCharges: transportCharges, checkBox: false, quantity: 0 });
    }
    $scope.quantitySelected = function (demand, index) {
        $scope.obj[index].quantity = demand;
    }
    $scope.sumTotal = function (index, checkedOrNot) {
        $scope.obj[index].checkBox = checkedOrNot;
        totalPrice = 0; totalDeliveryCharges = 0;
        for (var i = 0; i < $scope.cartCounter; i++) {
            if ($scope.obj[i].checkBox == true) {
                totalPrice = totalPrice + $scope.obj[i].price * $scope.obj[i].quantity;
                totalDeliveryCharges = totalDeliveryCharges + $scope.obj[i].deliveryCharges * $scope.obj[i].quantity;

            }
        }
      
        if (totalDeliveryCharges > $scope.FreeDeliveryLimit)
            totalDeliveryCharges = 0;
        $scope.totalPrice = totalPrice;
        $scope.totalDeliveryCharges = totalDeliveryCharges
    }
    $scope.loadQuantity = function (id, size, index) {
        for (var i = 0; i < $scope.cartCounter; i++) { $scope.loadQuantity[i] = [] };
        $http.get("/Customer/loadQuantity?id="+id + "&Size=" + size)
            .then(function (response) {
                var tempArray = []
                for (var i = 1; i <= response.data; i++) {
                    tempArray.push(i);
                }
                $scope.loadQuantity[index] = tempArray;
            }, function () { alert("Some Thing Went Wrong during Loading Quantity"); });
    }
    var stepProgress1 = jQuery("#progressStep1");
    var progressStep2 = jQuery("#progressStep2");
    $scope.step2Func = function () {
        $scope.step1 = false; $scope.step2 = true;
        progressStep2.addClass('current');
        stepProgress1.removeClass('current').addClass('done');
    }
    $scope.step1Func = function () {
        $scope.step1 = true; $scope.step2 = false;
        stepProgress1.addClass('current');
        progressStep2.removeClass('current');
    };
});