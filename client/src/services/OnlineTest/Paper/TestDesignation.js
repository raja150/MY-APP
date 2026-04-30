import APIService from '../../apiservice'
const apiUrl = 'OnlineTest/TestDesignation'

class TestDesignationService {

    async Paginate(data) {
        return await APIService.getAsync(`${apiUrl}/Paginate?${data}`)
    }

    async AddAsync(data) {
        return await APIService.postAsync(`${apiUrl}`, data)
    }

    async Delete(testDesId) {
        await APIService.putAsync(`${apiUrl}/Delete/${testDesId}`, {})
    }
}

export default new TestDesignationService