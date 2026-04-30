import APIService from '../../apiservice'
const apiUrl = 'OnlineTest/TestDepartment'

class TestDepartmentService {

    async Paginate(data) {
        return await APIService.getAsync(`${apiUrl}/Paginate?${data}`)
    }

    async AddAsync(data) {
        return await APIService.postAsync(`${apiUrl}`, data)
    }

    async Delete(testDeptId) {
        await APIService.putAsync(`${apiUrl}/Delete/${testDeptId}`, {})
    }
}

export default new TestDepartmentService