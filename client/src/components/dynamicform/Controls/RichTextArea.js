import JoditEditor from 'jodit-react';
import React, { useRef } from 'react';
import { FormGroup, Label } from 'reactstrap';


export default function RichTextArea(props) {
    const { value, height, width, placeholder, name, handlevaluechange,
        label, touched, error } = props

    const editor = useRef();
    const config = {
        readonly: false,
        placeholder: placeholder || 'Start typings...',
        height: height || '300px',//'300px',
        width: width || '100%',// '100%',
        enableDragAndDropFileToEditor: true,
        buttons: [
            '|', 'bold', 'italic', 'underline',
            '|', 'ul', 'ol',
            '|', 'font', 'fontsize', 'brush', 'paragraph',
            //'|', 'image', 'table',
            '|', 'cut', 'copy', 'paste',
            '|', 'left', 'center', 'right', 'justify',
            '|', 'undo', 'redo',
            '|', 'hr', 'eraser',
            '|', 'spellcheck',
            //'print',
            //'|', 'preview', //'find',
        ],
        uploader: { insertImageAsBase64URI: true },
        showXPathInStatusbar: false,
        showCharsCounter: false,
        toolbarAdaptive: true,
        toolbarSticky: true,
        statusbar: false,
        useNativeTooltip: true,
    };

    const handleOnBlur = (name, value) => {
        if (value.replace(/<[^>]+>/g, '')) {
            handlevaluechange(name, value)
        }
        else {
            handlevaluechange(name, '')
        }
    }


    return <FormGroup>
        <Label className="text-capitalize">{label}</Label>
        <JoditEditor
            ref={editor}
            value={value}
            config={config}
            tabIndex={1}
            onBlur={newContent => handleOnBlur(name, newContent)}
        />
        {touched && error ?
            <p style={{ color: 'red', fontSize: '11px' }}>{error}</p> : ''
        }
    </FormGroup>


};