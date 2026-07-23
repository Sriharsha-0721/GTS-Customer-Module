document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("WearNbr")
        ?.addEventListener("change", loadWearer);

    document.getElementById("btnNext")
        ?.addEventListener("click", loadNextWearer);

    document.getElementById("btnPrevious")
        ?.addEventListener("click", loadPreviousWearer);

    document.getElementById("btnSave")
        ?.addEventListener("click", saveWearer);

});
function populateWearer(data) {

    if (!data)
        return;

    document.getElementById("WearerId").value =
        data.wearerId;

    document.getElementById("WearNbr").value =
        data.wearNbr;

    document.getElementById("FirstName").value =
        data.firstName;

    document.getElementById("LastName").value =
        data.lastName;

    document.getElementById("Locker").value =
        data.locker;

    document.getElementById("LockRm").value =
        data.lockRm;

    document.getElementById("Sex").value =
        data.sex;
}
async function loadWearer() {

    const custId =
        document.getElementById("CustId").value;

    const wearNbr =
        document.getElementById("WearNbr").value;

    if (!wearNbr)
        return;

    const response =
        await fetch(`/api/Wearer?custId=${custId}&wearNbr=${wearNbr}`);

    if (!response.ok)
        return;

    const data =
        await response.json();

    populateWearer(data);
}
async function loadNextWearer() {

    const custId =
        document.getElementById("CustId").value;

    const wearerId =
        document.getElementById("WearerId").value;

    const response =
        await fetch(`/api/Wearer/Next?custId=${custId}&wearerId=${wearerId}`);

    if (!response.ok)
        return;

    populateWearer(await response.json());
}
async function loadPreviousWearer() {

    const custId =
        document.getElementById("CustId").value;

    const wearerId =
        document.getElementById("WearerId").value;

    const response =
        await fetch(`/api/Wearer/Previous?custId=${custId}&wearerId=${wearerId}`);

    if (!response.ok)
        return;

    populateWearer(await response.json());
}
async function saveWearer() {

    const dto = {

        wearerId:
            parseInt(document.getElementById("WearerId").value),

        locker:
            document.getElementById("Locker").value,

        lockRm:
            document.getElementById("LockRm").value,

        sex:
            document.getElementById("Sex").value === "true"

    };

    const response =
        await fetch("/api/Wearer", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify(dto)

        });

    if (response.ok) {

        alert("Wearer saved successfully.");

    }
    else {

        alert("Unable to save wearer.");

    }
}