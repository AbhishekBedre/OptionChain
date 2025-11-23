localStorage.setItem("darkMode", true);

const FILTERS = [
    "NIFTY 500",
    "NIFTY 50",
    "NIFTY 100",
    "NIFTY 200"
];

$(document).ready(function () {
    var dt = new Date();
    var hours = dt.getHours();
    var ampm = hours >= 12 ? "PM" : "AM";
    $("#spnLastUpdate").text("Last sync: " + getCurrentDateTime());

    updateFilterOptionsOnDashboard();
    var domain = window.location.origin;
    var subfolderName = "";

    var arr = window.location.pathname.split('/');

    if (arr.length > 2) {
        subfolderName = "/" + arr[1];
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(domain + subfolderName + "/notificationHub")
        .build();

    connection.on("NewStock", (name) => {
        playBeep();
        console.log("Signal R on NewStock event : " + name);
        getWatchlistIntradayStocks(function (response) {
            renderWatchListIntradayStocks(response);
        });
    });

    connection.on("DataUpdate", (name) => {
        console.log("Reload the data. Server has latest synced data.");
    });

    connection.start().catch(err => console.error(err));

    autoRefresh();

    var tokenResult = false;
    isTokenValid(localStorage.accessToken)
        .then(tokenResult => {
            if (tokenResult == false || (localStorage.userInfo == null
                || localStorage.userInfo == undefined
                || localStorage.accessToken == null)) {

                var url = domain + subfolderName + "/index.html";

                window.location.href = url;
            }
        });

    setTimeout(function () {
        var userDetails = JSON.parse(localStorage.userInfo);
        $("#profileImage").attr("src", userDetails.picture);
        $("#ProfileName").text(userDetails.name);
        document.dispatchEvent(new Event("click"));
    }, 200);

    $("#logout").click(function () {
        logout();
    });

    getIndexData();
});


function GenerateFilerOptions() {
    let options = ``;
    for (let i = 0; i < FILTERS.length; i++) {
        let j = i + 1;
        options += `<option value="${j}">${FILTERS[i]}</option>`;
    }
    return options;
}

function getCurrentDateTime() {
    var dt = new Date();

    var hours = dt.getHours();
    var minutes = dt.getMinutes();
    var seconds = dt.getSeconds();
    var ampm = hours >= 12 ? "PM" : "AM";

    hours = hours % 12;
    hours = hours ? hours : 12; // 0 → 12

    var strTime =
        hours +
        ":" +
        (minutes < 10 ? "0" + minutes : minutes) +
        ":" +
        (seconds < 10 ? "0" + seconds : seconds) +
        " " +
        ampm;

    var strDate =
        dt.getDate().toString().padStart(2, "0") +
        "-" +
        (dt.getMonth() + 1).toString().padStart(2, "0") +
        "-" +
        dt.getFullYear();

    return strDate + " " + strTime;
}

function updateFilterOptionsOnDashboard() {
    let options = GenerateFilerOptions();
    $("#stocksFilter").html(options);
}

async function isTokenValid(token) {
    try {
        const response = await fetch(`https://www.googleapis.com/oauth2/v3/tokeninfo?access_token=${token}`);
        const data = await response.json();

        if (response.ok) {
            return true;
        } else {
            console.warn("Token is invalid:", data);
            return false;
        }
    } catch (error) {
        console.error("Error verifying token:", error);
        return false;
    }
}

function logout() {
    if (localStorage.accessToken) {
        const revokeUrl = `https://accounts.google.com/o/oauth2/revoke?token=${localStorage.accessToken}`;

        // Revoke the token
        fetch(revokeUrl).then((response) => {
            if (response.ok) {
                accessToken = null;
                localStorage.clear();
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
                localStorage.clear();
                console.error('Error revoking token:', error);
            });
    } else {
        localStorage.clear();
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

    var url = domain + subfolderName + "/Options/major-index-v2?";

    $.ajax({
        url: url,
        type: 'GET',
        data: { "currentDate": selectedDate },
        success: function (response) {
            $("div[x-show='loaded']").hide();
            let nifty50 = response.find(s => s.name == "NIFTY 50");
            $("#nifty50").text(formatToThousands(nifty50.lastPrice.toFixed(2)));
            $("#niftyPer").text(nifty50.pChange + "%");
            $("#niftyVeri").text(nifty50.variation.toFixed(2));

            if (nifty50.pChange > 0) {
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

            let niftyBank = response.find(s => s.name == "NIFTY BANK");
            $("#niftyBank").text(formatToThousands(niftyBank.lastPrice.toFixed(2)));
            $("#niftyBankPer").text(niftyBank.pChange + "%");
            $("#niftyBankVeri").text(niftyBank.variation.toFixed(2));

            if (niftyBank.pChange > 0) {
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

            let niftyNext50 = response.find(s => s.name == "NIFTY NEXT 50");
            $("#niftyNext50").text(formatToThousands(niftyNext50.lastPrice.toFixed(2)));
            $("#niftyNext50Per").text(niftyNext50.pChange + "%");
            $("#niftyNext50Veri").text(niftyNext50.variation.toFixed(2));

            if (niftyNext50.pChange > 0) {
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

            let niftyMidSelect = response.find(s => s.name == "NIFTY MID SELECT");
            $("#niftyMidSelect").text(formatToThousands(niftyMidSelect.lastPrice.toFixed(2)));
            $("#niftyMidSelectPer").text(niftyMidSelect.pChange + "%");
            $("#niftyMidSelectVeri").text(niftyMidSelect.variation.toFixed(2));

            if (niftyMidSelect.pChange > 0) {
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

            let niftyFinService = response.find(s => s.name == "NIFTY FIN SERVICE");
            $("#niftyFinService").text(formatToThousands(niftyFinService.lastPrice.toFixed(2)));
            $("#niftyFinServicePer").text(niftyFinService.pChange + "%");
            $("#niftyFinServiceVeri").text(niftyFinService.variation.toFixed(2));

            if (niftyFinService.pChange > 0) {
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
            if (callback != null || callback != undefined) {
                return callback(response);
            }
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

function autoRefresh() {
    const now = new Date();
    const day = now.getDay();
    const start = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 9, 15); // 09:15 AM
    const end = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 15, 30);  // 03:30 PM

    // Prevent refresh on Saturday (6) and Sunday (0)
    if (day === 6 || day === 0) {
        console.log("No refresh on weekends.");
        return;
    }

    if (now >= start && now <= end) {
        // Calculate the next 5-minute slot
        // Calculate the next 5-minute slot
        const minutes = now.getMinutes();
        const nextSlot = Math.ceil((minutes + 1) / 5) * 5; // Ensures the next interval
        const nextRefresh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), nextSlot, 0, 0);

        // If the next slot moves to the next hour, adjust the hour accordingly
        if (nextSlot === 60) {
            nextRefresh.setHours(now.getHours() + 1);
            nextRefresh.setMinutes(0);
        }

        const timeToNextRefresh = nextRefresh - now;

        console.log(`Next refresh scheduled in ${timeToNextRefresh / 1000} seconds at ${nextRefresh.toLocaleTimeString()}`);

        setTimeout(function () {
            window.location.reload();
        }, timeToNextRefresh); // Reload at the start of the next 5-minute slot
    } else {
        console.log("Outside refresh hours.");
    }
}

let audioContext;

function initializeAudioContext() {
    if (!audioContext) {
        audioContext = new (window.AudioContext || window.webkitAudioContext)();
    }
    if (audioContext.state === "suspended") {
        audioContext.resume();
    }
}

document.addEventListener("click", initializeAudioContext, { once: true });
document.addEventListener("keydown", initializeAudioContext, { once: true });
document.addEventListener("touchstart", initializeAudioContext, { once: true });
document.addEventListener("pointerdown", initializeAudioContext, { once: true });
document.addEventListener("mousedown", initializeAudioContext, { once: true });

function playBeep() {
    if (!audioContext) {
        console.warn("Audio context not initialized. User interaction required.");
        return;
    }

    const oscillator = audioContext.createOscillator();
    const gainNode = audioContext.createGain();

    oscillator.type = "sine";
    oscillator.frequency.setValueAtTime(2000, audioContext.currentTime); // 1000 Hz beep
    oscillator.connect(gainNode);
    gainNode.connect(audioContext.destination);

    oscillator.start();
    setTimeout(() => oscillator.stop(), 300); // Stops after 300ms
}
