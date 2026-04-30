import APIService from '../apiservice';
class TaxDeclarationService {
    async postMyDeclaration(data, selfDeclaration) {
        if (data.id) {

            if (!selfDeclaration) {
                return await APIService.putAsync(`PayRoll/Declaration/Modify`, data)
            } else {
                return await APIService.putAsync(`PayRoll/Declaration`, data)
            }
        } else {
            if (!selfDeclaration) {
                return await APIService.postAsync(`PayRoll/Declaration/Save`, data)
            } else {
                return await APIService.postAsync(`PayRoll/Declaration`, data)
            }

        }
    }

    // async postDeclaration(data) {

    //     if (data.id) {
    //         return await APIService.putAsync(`PayRoll/Declaration/Modify`, data)
    //     } else {
    //         return await APIService.postAsync(`PayRoll/Declaration/Save`, data)

    //     }
    // }
    async getEightyC() {
        return await APIService.getAsync(`Payroll/Section80C/GetList?OrderBy=name`)
    }

    async getEightyD() {
        return await APIService.getAsync(`Payroll/Section80D/GetList?OrderBy=name`)
    }

    async getOtherSections() {
        return await APIService.getAsync(`Payroll/OtherSections/GetList?OrderBy=name`)
    }

    async getTaxDeclaration() {
        return await APIService.getAsync(`IncomeTax/ITDeclaration/GetList`)
    }
    paginate() {
        return { searchAPI: `PayRoll/Declaration/Paginate` }
    }
    async getEmpTaxDeclarationInfo(id) {
        return await APIService.getAsync(`PayRoll/Declaration/${id}`)
    }
    async getFinancialYear() {
        return await APIService.getAsync(`PayRoll/Declaration/OpenYears`)
    }
    async getMyDeclarationFY(id) {
        return await APIService.getAsync(`PayRoll/Declaration/MyDeclaration?id=${id}`)
    }

    async getPayrollDeclaration(id) {
        return await APIService.getAsync(`PayRoll/Declaration/Declaration/${id}`)
    }

    async DeclarationDownload(id, pwd) {
        return await APIService.getBlobAsync(`PayRoll/Declaration/TaxInfo/${id}/${pwd}`)
    }
    async getSettings(id) {
        return await APIService.getAsync(`PayRoll/Declaration/Settings/${id}`)
    }
    async CalculateForAll(id) {
        return await APIService.putAsync(`PayRoll/Declaration/CalculateForAll/${id}`)
    }
}
export default new TaxDeclarationService();