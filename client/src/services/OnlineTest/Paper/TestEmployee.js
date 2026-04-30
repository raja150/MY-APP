import APIService from '../../apiservice'
const apiUrl = 'OnlineTest/TestEmployee'

class TestEmployeeService {

    async Paginate(data) {
        return await APIService.getAsync(`${apiUrl}/Paginate?${data}`)
    }

    async AddAsync(data) {
        return await APIService.postAsync(`${apiUrl}`, data)
    }

    async Delete(testEmpId) {
        await APIService.putAsync(`${apiUrl}/Delete/${testEmpId}`, {})
    }

    
}
export default new TestEmployeeService