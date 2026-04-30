import APIService from '../apiservice'
const apiUrl = 'HelpDesk/DeskDepartment'

class DeskDepartmentService{
    async getList(){
        return await APIService.getAsync(`${apiUrl}/Depts`)
    }
    async getDepList(){
        return await APIService.getAsync(`${apiUrl}/GetDepList`)
    }
}

export default new DeskDepartmentService()
