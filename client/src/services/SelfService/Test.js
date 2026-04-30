import APIService from '../apiservice'
const apiUrl = 'SelfService/Test'
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

    async Start(testId) {
        return await APIService.getAsync(`${apiUrl}/Start/${testId}`)
    }

    async GetAnswers(testId) {
        return await APIService.getAsync(`${apiUrl}/${testId}`)
    }

    async IsFinished(testId) {
        return await APIService.getAsync(`${apiUrl}/IsAllowed/${testId}`)
    }

    paginate() {
        return {
            searchAPI: `${apiUrl}/Paginate`
        }
    }

    async GetList() {
        return await APIService.getAsync(`${apiUrl}/List`)
    }

}
export default new TestService()