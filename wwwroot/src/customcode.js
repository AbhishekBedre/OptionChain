$(document).ready(function(){

    if(sessionStorage.userInfo == null || sessionStorage.userInfo == undefined || sessionStorage.accessToken == null) {
        var domain = window.location.origin;
        var subfolderName = "";

        var arr = window.location.pathname.split('/');

        if (arr.length > 2) {
            subfolderName = "/" + arr[1];
        }

        var url = domain + subfolderName + "/index.html";

        window.location.href = url;
    }

    var userDetails = JSON.parse(sessionStorage.userInfo);
    $("#profileImage").attr("src", userDetails.picture);
    $("#ProfileName").text(userDetails.name);

    $("#logout").click(function(){
        logout();
    });

    getIndexData();

});

function logout() {
    if (sessionStorage.accessToken) {
        const revokeUrl = `https://accounts.google.com/o/oauth2/revoke?token=${sessionStorage.accessToken}`;
        
        // Revoke the token
        fetch(revokeUrl)
        .then((response) => {
            if (response.ok) {
                accessToken = null;
                sessionStorage.clear();
                window.location.hash = ''; // Clear the URL hash
            } 

            var domain = window.location.origin;
            var subfolderName = "";

            var arr = window.location.pathname.split('/');

            if (arr.length > 2) {
                subfolderName = "/" + arr[1];
            }

            var url = domain + subfolderName + "/index.html";
            
            window.location.href = url;
        })
        .catch((error) => {
            sessionStorage.clear();
            console.error('Error revoking token:', error);
        });
    } else {
        sessionStorage.clear();
    }
}

function getIndexData(callback) {

    $("div[x-show='loaded']").show();

    var domain = window.location.origin;

    var subfolderName = "";

    var arr = window.location.pathname.split('/');

    if (arr.length > 2) {
        subfolderName = "/" + arr[1];
    }

    var selectedDate = $("#currentDate").val();

    var url = domain + subfolderName + "/Options/major-index?";

    $.ajax({
        url: url,
        type: 'GET',
        data: { "currentDate": selectedDate },
        success: function (response) {
            $("div[x-show='loaded']").hide();
            let nifty50 = response.find(s=>s.name == "NIFTY 50");
            $("#nifty50").text(formatToThousands(nifty50.lastPrice));
            $("#niftyPer").text(nifty50.pChange + "%");
            $("#niftyVeri").text(nifty50.variation);

            if(nifty50.pChange > 0) {
                $("#nifty50").removeClass("text-red");
                $("#nifty50").addClass("text-meta-3");    
                
                $("#niftyIcon").attr("d", "M8.25259 5.87281L4.22834 9.89706L3.16751 8.83623L9.00282 3.00092L14.8381 8.83623L13.7773 9.89705L9.75306 5.87281L9.75306 15.0046L8.25259 15.0046L8.25259 5.87281Z");
                $("#niftyIcon").attr("fill", "#10B981");
                
                $("#niftyPer").removeClass("text-red");
                $("#niftyPer").addClass("text-meta-3");

                $("#niftyVeri").removeClass("text-red");
                $("#niftyVeri").addClass("text-meta-3");                
            }
            else {
                $("#nifty50").removeClass("text-meta-3");   
                $("#nifty50").addClass("text-red");

                $("#niftyIcon").attr("d", "M9.75302 12.1328L13.7773 8.10856L14.8381 9.16939L9.00279 15.0047L3.16748 9.16939L4.22831 8.10856L8.25256 12.1328V3.00098H9.75302V12.1328Z");
                $("#niftyIcon").attr("fill", "#ee544e");
                
                $("#niftyPer").removeClass("text-meta-3");
                $("#niftyPer").addClass("text-red");

                $("#niftyVeri").removeClass("text-meta-3");
                $("#niftyVeri").addClass("text-red");
            }

            let niftyBank = response.find(s=>s.name == "NIFTY BANK");
            $("#niftyBank").text(formatToThousands(niftyBank.lastPrice));
            $("#niftyBankPer").text(niftyBank.pChange + "%");
            $("#niftyBankVeri").text(niftyBank.variation);

            if(niftyBank.pChange > 0) {
                $("#niftyBank").removeClass("text-red");
                $("#niftyBank").addClass("text-meta-3");    
                
                $("#niftyBankIcon").attr("d", "M8.25259 5.87281L4.22834 9.89706L3.16751 8.83623L9.00282 3.00092L14.8381 8.83623L13.7773 9.89705L9.75306 5.87281L9.75306 15.0046L8.25259 15.0046L8.25259 5.87281Z");
                $("#niftyBankIcon").attr("fill", "#10B981");
                
                $("#niftyBankPer").removeClass("text-red");
                $("#niftyBankPer").addClass("text-meta-3");

                $("#niftyBankVeri").removeClass("text-red");
                $("#niftyBankVeri").addClass("text-meta-3");   
            } else {
                $("#niftyBank").removeClass("text-meta-3");   
                $("#niftyBank").addClass("text-red");

                $("#niftyBankIcon").attr("d", "M9.75302 12.1328L13.7773 8.10856L14.8381 9.16939L9.00279 15.0047L3.16748 9.16939L4.22831 8.10856L8.25256 12.1328V3.00098H9.75302V12.1328Z");
                $("#niftyBankIcon").attr("fill", "#ee544e");
                
                $("#niftyBankPer").removeClass("text-meta-3");
                $("#niftyBankPer").addClass("text-red");

                $("#niftyBankVeri").removeClass("text-meta-3");
                $("#niftyBankVeri").addClass("text-red");
            }

            let niftyNext50 = response.find(s=>s.name == "NIFTY NEXT 50");
            $("#niftyNext50").text(formatToThousands(niftyNext50.lastPrice));
            $("#niftyNext50Per").text(niftyNext50.pChange + "%");
            $("#niftyNext50Veri").text(niftyNext50.variation);

            if(niftyNext50.pChange > 0) {
                $("#niftyNext50").removeClass("text-red");
                $("#niftyNext50").addClass("text-meta-3");    
                
                $("#niftyNext50Icon").attr("d", "M8.25259 5.87281L4.22834 9.89706L3.16751 8.83623L9.00282 3.00092L14.8381 8.83623L13.7773 9.89705L9.75306 5.87281L9.75306 15.0046L8.25259 15.0046L8.25259 5.87281Z");
                $("#niftyNext50Icon").attr("fill", "#10B981");
                
                $("#niftyNext50Per").removeClass("text-red");
                $("#niftyNext50Per").addClass("text-meta-3");

                $("#niftyNext50Veri").removeClass("text-red");
                $("#niftyNext50Veri").addClass("text-meta-3");
            } else {
                $("#niftyNext50").removeClass("text-meta-3");   
                $("#niftyNext50").addClass("text-red");

                $("#niftyNext50Icon").attr("d", "M9.75302 12.1328L13.7773 8.10856L14.8381 9.16939L9.00279 15.0047L3.16748 9.16939L4.22831 8.10856L8.25256 12.1328V3.00098H9.75302V12.1328Z");
                $("#niftyNext50Icon").attr("fill", "#ee544e");
                
                $("#niftyNext50Per").removeClass("text-meta-3");
                $("#niftyNext50Per").addClass("text-red");

                $("#niftyNext50Veri").removeClass("text-meta-3");
                $("#niftyNext50Veri").addClass("text-red");
            }

            let niftyMidSelect = response.find(s=>s.name == "NIFTY MIDCAP SELECT");
            $("#niftyMidSelect").text(formatToThousands(niftyMidSelect.lastPrice));
            $("#niftyMidSelectPer").text(niftyMidSelect.pChange + "%");
            $("#niftyMidSelectVeri").text(niftyMidSelect.variation);

            if(niftyMidSelect.pChange > 0) {
                $("#niftyMidSelect").removeClass("text-red");
                $("#niftyMidSelect").addClass("text-meta-3");    
                
                $("#niftyMidSelectIcon").attr("d", "M8.25259 5.87281L4.22834 9.89706L3.16751 8.83623L9.00282 3.00092L14.8381 8.83623L13.7773 9.89705L9.75306 5.87281L9.75306 15.0046L8.25259 15.0046L8.25259 5.87281Z");
                $("#niftyMidSelectIcon").attr("fill", "#10B981");
                
                $("#niftyMidSelectPer").removeClass("text-red");
                $("#niftyMidSelectPer").addClass("text-meta-3");

                $("#niftyMidSelectVeri").removeClass("text-red");
                $("#niftyMidSelectVeri").addClass("text-meta-3");
            } else {
                $("#niftyMidSelect").removeClass("text-meta-3");   
                $("#niftyMidSelect").addClass("text-red");

                $("#niftyMidSelectIcon").attr("d", "M9.75302 12.1328L13.7773 8.10856L14.8381 9.16939L9.00279 15.0047L3.16748 9.16939L4.22831 8.10856L8.25256 12.1328V3.00098H9.75302V12.1328Z");
                $("#niftyMidSelectIcon").attr("fill", "#ee544e");
                
                $("#niftyMidSelectPer").removeClass("text-meta-3");
                $("#niftyMidSelectPer").addClass("text-red");

                $("#niftyMidSelectVeri").removeClass("text-meta-3");
                $("#niftyMidSelectVeri").addClass("text-red");
            }

            let niftyFinService = response.find(s=>s.name == "NIFTY FINANCIAL SERVICES");
            $("#niftyFinService").text(formatToThousands(niftyFinService.lastPrice));
            $("#niftyFinServicePer").text(niftyFinService.pChange + "%");
            $("#niftyFinServiceVeri").text(niftyFinService.variation);

            if(niftyFinService.pChange > 0) {
                $("#niftyFinService").removeClass("text-red");
                $("#niftyFinService").addClass("text-meta-3");    
                
                $("#niftyFinServiceIcon").attr("d", "M8.25259 5.87281L4.22834 9.89706L3.16751 8.83623L9.00282 3.00092L14.8381 8.83623L13.7773 9.89705L9.75306 5.87281L9.75306 15.0046L8.25259 15.0046L8.25259 5.87281Z");
                $("#niftyFinServiceIcon").attr("fill", "#10B981");
                
                $("#niftyFinServicePer").removeClass("text-red");
                $("#niftyFinServicePer").addClass("text-meta-3");

                $("#niftyFinServiceVeri").removeClass("text-red");
                $("#niftyFinServiceVeri").addClass("text-meta-3");
            }
            else {
                $("#niftyFinService").removeClass("text-meta-3");   
                $("#niftyFinService").addClass("text-red");

                $("#niftyFinServiceIcon").attr("d", "M9.75302 12.1328L13.7773 8.10856L14.8381 9.16939L9.00279 15.0047L3.16748 9.16939L4.22831 8.10856L8.25256 12.1328V3.00098H9.75302V12.1328Z");
                $("#niftyFinServiceIcon").attr("fill", "#ee544e");
                
                $("#niftyFinServicePer").removeClass("text-meta-3");
                $("#niftyFinServicePer").addClass("text-red");

                $("#niftyFinServiceVeri").removeClass("text-meta-3");
                $("#niftyFinServiceVeri").addClass("text-red");
            }
            return callback(response);
        },
        error: function (xhr, status, error) {
            $("div[x-show='loaded']").hide();
            console.error('Error:', status, error);
        }
    });
}

function formatToThousands(decimalNumber) {
    if (isNaN(decimalNumber)) {
        throw new Error("Input must be a valid number");
    }

    return decimalNumber.toLocaleString("en-US");
}