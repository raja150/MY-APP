import APIService from "./apiservice";

const apiUrl = 'Image'

class ImageService {
    async getImage() {
        return await APIService.getAsync(`${apiUrl}`)
    }
    async UpdateAsync(data) {
        return await APIService.putAsync(`${apiUrl}`, data)
    }
    async PostAsync(data) {
        return await APIService.postAsync(`${apiUrl}`, data)
    }
}
export default new ImageService();