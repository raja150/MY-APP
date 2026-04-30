export  function HRAEmptyItem() {
    return {
        rentalFrom: '',
        rentalTo:'',
        amount: 0,
        address: '',
        city:'',
        pan: '', 
        landlord:'',
        invalid: false,
        isDeleted: false
    };
}
export  function LetOutEmptyItem(){
    return{
        // id:'',
        annualRentReceived : 0,
        municipalTaxPaid:0,
        netAnnualValue:0,
        standardDeduction:0,
        repayingHomeLoan:false,
        interestPaid:0,
        nameOfLender:'',
        lenderPAN:'',
        principle:0,
        netIncome:0,
    }
}
export function EightyCItem(){
    return{ 
        section80CId:'',
        amount:0,
    }
}
export function EightyDItem(){
    return{
        //id:'',
        // medicalClaim:null,
        section80DId: '',
        // amount : 0,
        medicalClaimAmount:0,
        limit : 0
    }
}
export function OtherEmptyItem(){
    return{
        //id:'',
        // selectInvestment:null,
        otherSectionsId : '',
        amount : 0,
        // investmentAmount:0,
        limit:0
    }
}