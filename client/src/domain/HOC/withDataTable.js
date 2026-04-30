
import _ from 'lodash';
import queryString from 'query-string';
import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import SessionStorageService from 'services/SessionStorage';
import APIService from '../../services/apiservice';
import * as crypto from '../../utils/Crypto';
import * as formUtil from '../../utils/form';

export default function WithDataTable(WrappedComponent, val, fields, ddData, title, onEdit, admission, type) {


    function HOC(props) {
        const size = SessionStorageService.getPageSize()
        const queryProps = { ...queryString.parse(props.location.search) };

        const [searchData, setSearchData] = useState({})

        const [state, setFrmState] = useState({
            isLoading: true, columns: [],
            data: [], pages: -1, title: '', pageSize: size || 10, count: 0
        })

        const [tableLoading, setTableLoading] = useState(false);

        const history = useHistory();

        // Open modal when click on edit button
        const [modal, setModal] = useState(false);
        const [modalId, setModalId] = useState({});


        useEffect(() => {
            const fetchData = async () => {
                const cols = formUtil.getTableColsFromJson(fields, ddData);
                //Update the search state
                setSearchData(queryProps);
                const apiData = await getData(queryProps, size || state.pageSize)
                setFrmState({
                    ...state, isLoading: false, title: title,
                    columns: cols,
                    data: apiData.data, pages: apiData.pages,
                    hasNext: apiData.hasNext,
                    hasPrevious: apiData.hasPrevious,
                    pageIndex: apiData.pageIndex,
                    pageSize: apiData.pageSize,
                    count: apiData.count
                });

            };
            fetchData();
        }, []);

        const getData = async (query, size) => {
            setTableLoading(true)
            let pages = '', hasNext = false, hasPrevious = false,
                pageIndex = 0, data = [], pageSize = 10, count = 0;

            const qString = queryString.stringify({
                ...query,
                size: size || 10,
                page: query['page'] || 0
            });

            const params = new URLSearchParams();
            Object.keys(query).forEach(function (key) {
                params.append(key, query[key])
            });

            SessionStorageService.setPageSize(size)
            history.push({ search: params.toString() });

            await APIService.getAsync(`${val.searchAPI}?${qString}`).then((res) => {
                data = res.data.items;
                pages = res.data.pages;
                hasNext = res.data.hasNext;
                hasPrevious = res.data.hasPrevious;
                pageIndex = res.data.index
                pageSize = res.data.size
                count = res.data.count
            }).catch(err => {
                formUtil.displayAPIError(err)
            });
            setTableLoading(false)
            return {
                data: data,
                pages: pages,
                pageIndex: pageIndex,
                hasNext: hasNext,
                hasPrevious: hasPrevious,
                pageSize: pageSize,
                count: count
            }
        }


        const handleEdit = (n) => {
            if (onEdit) {
                setModalId(n)
                admission == "" && toggle()
            } else {
                const qry = { r: (n.id ? crypto.encrypt(n.id) : '') };
                props.history.push(`${props.match.path}/Update?` + queryString.stringify(qry));
            }
        }



        const toggle = () => {
            setModal(!modal)
        }

        const handleOnChange = (name, value) => {
            setSearchData({ ...searchData, [name]: value });
        }

        const handleSearch = async (page, pageSize, sortBy, isDescend) => {
            //Push search parameters into router history
            searchData['page'] = page || 0
            searchData['sortBy'] = sortBy ? sortBy : queryProps['sortBy'] ? queryProps['sortBy'] : 'AddedAt'
            searchData['isDescend'] = isDescend != undefined ? isDescend : queryProps['isDescend'] ? queryProps['isDescend'] : false
            const apiData = await getData(searchData, pageSize || size)
            setFrmState({
                ...state,
                data: apiData.data,
                pages: apiData.pages,
                hasNext: apiData.hasNext,
                hasPrevious: apiData.hasPrevious,
                pageIndex: apiData.pageIndex,
                pageSize: apiData.pageSize,
                count: apiData.count
            });
        }

        return (
            <WrappedComponent
                searchData={searchData}
                state={state}
                tableLoading={tableLoading}
                modal={modal}
                modalData={modalId}
                type={type}
                handleOnChange={handleOnChange}
                handleSearch={handleSearch}
                handleEdit={handleEdit}
                toggle={toggle}
                {...props} />
        )
    }
    return HOC;
}