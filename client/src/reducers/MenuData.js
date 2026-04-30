export const SET_MENU_DATA = 'SET_MENU_DATA'

export const setMenuData = (data) => {
    return {
        type: SET_MENU_DATA,
        payload: data
    }
}

const initialState = {
    menuData : []
}

export default function MenuReducer(state = initialState, action) {
    switch (action.type) {
        case SET_MENU_DATA:
            return {
                ...state,
                menuData: action.payload
            };
        default:
            return {
                ...state
            }
    }
}