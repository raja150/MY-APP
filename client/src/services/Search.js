import APIService from "./apiservice";

const apiUrl = 'Search'
class SearchService {
    async WithEmployeeName(name) {
        return await APIService.getAsync(`${apiUrl}/SearchByEmployee/${name}`)
    }

    async SearchWithEmpName(name) {
        return await APIService.getAsync(`${apiUrl}/Employee/${name}`)
    }
    async SearchWithEmpTeam(name) {
        return await APIService.getAsync(`${apiUrl}/SearchTeamEmployee/${name}`)
    }
    async HelpDeskSearch(name, raisedById) {
        return await APIService.getAsync(`${apiUrl}/HelpDesk/${name}/${raisedById}`)
    }
    async DeskGroupEmps(deptId, name) {
        return await APIService.getAsync(`${apiUrl}/DeskGroupEmps/${name}/${deptId}`)
    }

    async ResultPaperSearchWithName(name) {
        return await APIService.getAsync(`${apiUrl}/ResultPaper/${name}`)
    }
    async ResultEmpSearchWithName(name) {
        return await APIService.getAsync(`${apiUrl}/ResultEmp/${name}`)
    }

    async ResultPaperSearchWithEmpId(name, EmpId) {
        return await APIService.getAsync(`${apiUrl}/ResultPaperByEmpId/${name}/${EmpId}`)
    }
    async GetPaperSearchWithName(name) {
        return await APIService.getAsync(`${apiUrl}/Paper/${name}}`)
    }
}
export default new SearchService();