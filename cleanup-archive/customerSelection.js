function saveSelectedCustomer(customer) {

    localStorage.setItem("MC", customer.mc);
    localStorage.setItem("CustID", customer.custId);
    localStorage.setItem("CustNo", customer.custNo);
    localStorage.setItem("Customer", customer.customer);
    localStorage.setItem("Route", customer.route);
    localStorage.setItem("GID", customer.gid);
    localStorage.setItem("City", customer.city);
    localStorage.setItem("Phone", customer.phone);
    localStorage.setItem("Contact", customer.contact);
    localStorage.setItem("Address1", customer.address1);
    localStorage.setItem("Address2", customer.address2);
}

function getSelectedCustomer() {

    return {

        MC: localStorage.getItem("MC"),
        CustID: localStorage.getItem("CustID"),
        CustNo: localStorage.getItem("CustNo"),
        Customer: localStorage.getItem("Customer"),
        Route: localStorage.getItem("Route"),
        GID: localStorage.getItem("GID"),
        City: localStorage.getItem("City"),
        Phone: localStorage.getItem("Phone"),
        Contact: localStorage.getItem("Contact"),
        Address1: localStorage.getItem("Address1"),
        Address2: localStorage.getItem("Address2")
    };
}

function clearSelectedCustomer() {

    localStorage.clear();
}