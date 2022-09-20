
function answerReview(index, reviewId, seller) {
    var answer = { Text: $("#reviewAnsbtn_" + index).next("textarea").val(), FeedbackId: reviewId, User: seller };
    $.ajax({
        url: "/Api/CustomerApi/AnswerReview",
        data: JSON.stringify(answer),
        method: "POST",
        contentType: "application/json",
        success: function (data) {
            var html = '<div class="col-11 seller-response">' +
                '<div class="arrow-up"></div>' +
                '<div class="mic-info col-12"><span class="fa fa-briefcase text-danger">&emsp;</span><span class="text-danger">Seller Response -with 21 hours</span></div>' +
                '<div class="comment-text col-12">' + data + '</div>' +
                '</div>';
            $("#reviewAnsbtn_" + index).parent().append(html);
            toastr.success("review answered successfully")
        },
        error: function () {
            toastr.warning("something went wrong during answering Review");
        }
    })
}

function addImage() {
    var html = '<div class="form-group row">' +
        '<label class="badge text-right mt-1 col-md-2 col-3" > Parent Image</label>' +
        '<div class="col-6">' +
        '<input type="file" id="userImage" name="ProductImage" class="form-control" accept="image/*" />' +
        '</div>' +
        '</div >';
    $(html).insertAfter($("#seconderyImages"));
}
function openTextAreaForReview(index, reviewId, seller) {
    var temp = $("#reviewAnsbtn_" + index);
    var textarea = '<textarea class="form-control col-7 mx-3 mb-3" placeholder="Answar Customer..."></textarea>' +
        '<div class="align-self-center"><button class="btn text-success fas fa-arrow-circle-right fa-2x" onclick="answerReview(' + index + ',' + reviewId + ',\'' + seller + '\')"></button></div>';
    $(textarea).insertAfter(temp);
    temp.hide();
}
function SizeandQuantityEditHtml(size, count) {
    if (size != null && size != "No Size") {
        var html = '<div class="row mt-2">' +
            '<label class="badge col-2 text-right mt-1">Size</label>' +
            '<div class="col-3">' +
            '<input id="Prod_Sizes_' + count + '__AllSizes" name="Prod.Sizes[' + count + '].AllSizes" type="hidden" value="' + size + '"/>' +
            '<p class="form-control">' + size + '</p>' +
            '</div>' +
            '<label class="badge col-2 text-right mt-1">Quantity</label>' +
            '<div class="col-3">' +
            '<input type="number"  class="form-control" id="Prod_Sizes_' + count + '__Quantity" name="Prod.Sizes[' + count + '].Quantity"/>' +
            '</div>' +
            '</div>';
        $("#AddSize").append(html);
    }
}
function askQuestion(productId, loginUser) {
    var date = dateFormate(new Date());
    var question = { Text: $("#askQuestion").children("textarea").val(), FeedbackId: productId, User: loginUser };
    $.ajax({
        url: "/Api/CustomerApi/AskQuestion",
        data: JSON.stringify(question),
        contentType: "application/json",
        method: "POST",
        success: function (data) {
            var QuestionHtml = '<div class="bg-warning rounded p-2 mb-2"><div class="mic-info">By: <a href="#">' + $("#sessionName").text() + '</a> ' + date + '</div>' +
                '<div class="comment-text">' + data + '</div>';
            $("#Answar").append(QuestionHtml);
            $("#askQuestion").children("textarea").val("");
            toastr.success("question posted successfully the seller will respond when available");
        },
        error: function () {
            toastr.warning("something went wrong during Asking Queries");
        }
    })
}
function answerQuestion(id, seller, index) {
    var temp = $("#Ansbtn_" + index);
    var Ans = { Text: temp.next("textarea").val(), FeedbackId: id, User: seller };
    $.ajax({
        url: "/Api/CustomerApi/AnswerQuestion",
        data: JSON.stringify(Ans),
        contentType: "application/json",
        method: "POST",
        success: function (data) {
            var AnsHtml = '<div class="col-11 seller-response">' +
                '<div class="arrow-up"></div>' +
                '<div class="mic-info col-12"><span class="fa fa-briefcase text-danger">&emsp;</span><span class="text-danger">Seller Response -with 21 hours</span></div>' +
                '<div class="comment-text col-12">' + data + '</div>' +
                '</div>';
            temp.next("textarea").remove();
            temp.next("div").remove();
            $(AnsHtml).insertAfter(temp);
            toastr.success("Answered Successfully!");
        },
        error: function () {
            toastr.warning("something went wrong during Answering the Customer");
        }
    })
}
function openTextArea(index, id, seller) {
    var temp = $("#Ansbtn_" + index);
    var textarea = '<textarea class="form-control col-7 mx-3 mb-3" placeholder="Answar Customer..."></textarea>' +
        '<div class="align-self-center"><button class="btn text-success fas fa-arrow-circle-right fa-2x" onclick="answerQuestion(' + id + ',\'' + seller + '\',' + index + ')"></button></div>';
    $(textarea).insertAfter(temp);
    temp.hide();
}
function listOfAllSizes() {
    var arr = ["--No Size--"];

    listOfShoesSizes().forEach(function (value) { arr.push(value) });
    listOfShirtsSizes().forEach(function (value) { arr.push(value) });
    return arr;

}
function listOfJeansSizes() {
    var arr = ["28", "30", "32", "34", "36", "38", "40"];
    return arr;
}
function listOfShirtsSizes() {
    var arr = ["XS", "S", "M", "L", "XL", "XXL", "XXXL"];
    return arr;
}
function listOfShoesSizes() {
    var British = ["5", "5.5", "6", "6.5", "7", "7.5", "8", "8.5", "9", "9.5", "10", "10.5", "11", "11.5", "12"];
    return British;
}
function fillStar(limit) {
    for (var i = 1; i <= limit; i++) {
        var idString = i.toString();
        var idPointer = "#SvgStarFillEmpty-" + idString;
        $(idPointer + " .Fill").show();
    }
}
function clearStars() {
    $(".Fill").hide();
}
function rateProduct(rateMarks) {
    clearStars = false;
    fillStar = false;
    for (var i = 1; i <= rateMarks; i++) {
        var idString = i.toString();
        var idPointer = "#SvgStarFillEmpty-" + idString;
        $(idPointer + " .Fill").show();
    }
    for (var i = rateMarks + 1; i <= 5; i++) {
        var idString = i.toString();
        var idPointer = "#SvgStarFillEmpty-" + idString;
        $(idPointer + " .Fill").hide();
    }
    document.getElementById("ratingStars").value = rateMarks;
}
function dateFormate(str) {
    var now = new Date();
    var date = new Date(str);
    if (now.getDay() - date.getDay() == 0 && now.getMonth() - date.getMonth() == 0 && now.getFullYear() - date.getFullYear() == 0) {
        var h = date.getHours();
        if (h > 12)
            h = (h - 12) + "PM";
        else if (h == 0)
            h = 12 + "PM";
        else h + "AM";
        var m = date.getMinutes();
        var s = date.getSeconds();
        return h + ":" + m + ":" + s;
    }
    var Day = date.getDate();
    var month = date.getMonth();
    var year = date.getFullYear();
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    month = months[month];
    var fullDate = Day + '-' + month + '-' + year;
    return fullDate;
};
function dateSubtract(Date1, Date2) {
    var date1 = new Date(Date1); var date2 = new Date(Date2);
    var month1 = date1.getMonth(); var month2 = date2.getMonth();
    var year2 = date2.getFullYear();
    var Day1 = date1.getDate();
    var Day2 = date2.getDate();
    var yearDifference = year2 - date1.getFullYear();
    var monthDifference = month2 - month1;
    var dayDifference = Day2 - Day1;
    var hourDifference = date2.getHours() - date1.getHours();
    var minuteDifference = date2.getMinutes() - date1.getMinutes();
    var daysinMonths = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    if (year2 % 4 == 0) daysinMonths[1] = 29;
    daysinMonthsDiff = daysinMonths[month1];
    if (yearDifference >= 1) {
        if (monthDifference >= 0)
            return yearDifference + " years " + monthDifference + " months";
        else if (monthDifference < 0 && yearDifference == 1)
            return monthDifference + 12 + " months";
        else
            return yearDifference - 1 + " years " + monthDifference + 12 + " months";

    }
    else if (monthDifference >= 1) {
        var monthChangeHourCheck = daysinMonths[month2 - 1] - Day1;
        if (dayDifference >= 0)
            return monthDifference + " months " + dayDifference + " days";
        else if (monthDifference > 1 && dayDifference < 0)
            return monthDifference + " months " + dayDifference + daysinMonthsDiff + "days";
        else if (monthDifference == 1 && monthChangeHourCheck == 0 && hourDifference < 0) {
            if (hourDifference < 0)
                return hourDifference + 24 + " hours ";
            else return hourDifference + " hours ";
        }
        else
            return dayDifference + daysinMonthsDiff + " days";
    }
    else if (dayDifference >= 1) {
        if (hourDifference >= 0)
            return dayDifference + " days " + hourDifference + " hours ";
        else if (dayDifference == 1 && hourDifference < 0)
            return hourDifference + 24 + " hours";
        else return dayDifference + daysinMonthsDiff + " days " + hourDifference + " hours";
    }
    else if (hourDifference >= 1) {
        if (minuteDifference >= 0)
            return hourDifference + " hours " + minuteDifference + " minutes";
        else if (hourDifference == 1 && minuteDifference < 0)
            return minuteDifference + 60 + " minutes";
        else return hourDifference + 24 + " hours " + minuteDifference + " minutes";
    }
    else if (minuteDifference > 1) return minuteDifference + " Minutes";
    else return " Instantly";
}
function showSlides(n, slideIndex) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace("active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += "active";
    captionText.innerHTML = dots[slideIndex - 1].alt;
    return slideIndex;
}
function AddToCart(id, quickShop) {
    $.ajax({
        url: '/Home/AddToCart?ProductId=' + id,
        method: "POST",
        success: function (data) {
            if (data == "Added") {
                toastr.success("Successfully Added To Cart");
                var cart = $("i span.cartLook").text();
                cart = parseInt(cart);
                if (!cart) cart = 0;
                cart++;
                $(".cartLook").text(cart);
            }
            else { if (quickShop == false) toastr.warning("Already Exist in Cart"); }
            if (quickShop == true) {
                window.location.href = "/Customer/CartCollection/";
            }
        },
        error: function () {
            toastr.warning("product not load to cart");
        }
    })
}
function LinearRating(rating, str) {
    rating++;
    for (var i = 1; i <= rating; i++) {
        var decimal = rating - i;
        var idPointer = "#SvgStarLinear-" + str + "-" + i.toString();
        if (decimal >= 0.8) {
            $(idPointer + " .FullFill").show();
        }
        else if (decimal >= 0.6) {
            $(idPointer + " .TwoByThird").show();
        }
        else if (decimal >= 0.4) {
            $(idPointer + " .HalfFill").show();
        }
        else if (decimal >= .2) {
            $(idPointer + " .OneByThird").show();
        }
    }
}
