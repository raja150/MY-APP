import APIService from '../apiservice';
class RentalService{
    async postHomeRental(data) {
        
        if (data.id) {
            return await APIService.putAsync(`PayRoll/TaxDeclaration`, data)
        } else {
            return await APIService.postAsync(`PayRoll/TaxDeclaration`, data) 
        }
    }
    async getEightyC(){
        return await APIService.getAsync(`IncomeTax/Section80C/GetList`)
    }

    async getEightyD(){
        return await APIService.getAsync(`IncomeTax/Section80D/GetList`)
    }

    async getOtherSections(){
        return await APIService.getAsync(`IncomeTax/OtherSections/GetList`)
    } 

    async getITDeclarations(){
        return await APIService.getAsync(`IncomeTax/ITDeclaration/GetList`)
    }
    paginate(){
        return {searchAPI:`PayRoll/TaxDeclaration/Paginate`}
    }
    async getEmpRentInfo(id){
        return await APIService.getAsync(`PayRoll/TaxDeclaration/${id}`)
    }
    async getLeavesInfo(){
        return await APIService.getAsync(`SelfService/Report/Leaves`)
    }
    
    async getEmployeeLeavesInfo(){
        return await APIService.getAsync(`SelfService/Report/EmployeeLeaves`)
    }
    async employeeProfile(){
        return await APIService.getAsync(`SelfService/Report/EmployeeProfile`)
    }
    async CD_DayPerformance(){
        return await APIService.getAsync(`SelfService/Report/CD_DayPerformance`)
    }
    async CD_MonthPerformance(){
        return await APIService.getAsync(`SelfService/Report/CD_MonthPerformance`)
    }
    async MT_PerformancePayCut(){
        return await APIService.getAsync(`SelfService/Report/MT_PerformancePayCut`)
    }
    async CD_PerformancePayCut(){
        return await APIService.getAsync(`SelfService/Report/CD_PerformancePayCut`)
    }
}
export default new RentalService();