import APIService from '../apiservice'
const apiUrl = 'OnlineTest/Test'
class TestService {

    async updateAsync(data) {
        if (data.id) {
            await APIService.putAsync(apiUrl, data);
        } else {
            await APIService.postAsync(apiUrl, data);
        }
    }

    async Save(data) {
        await APIService.postAsync(`${apiUrl}/SaveTest`, data);
    }

    async Start(paperId){
        return await APIService.getAsync(`${apiUrl}/Start/${paperId}`)
    }

    async GetAnswers(testId) {
        return await APIService.getAsync(`${apiUrl}/${testId}`)
    }
    
    paginate() {
        return {
            searchAPI: `${apiUrl}/Paginate`
        }
    }

    async GetList(){
        return await APIService.getAsync(`${apiUrl}/List`)
    }

}
export default new TestService()