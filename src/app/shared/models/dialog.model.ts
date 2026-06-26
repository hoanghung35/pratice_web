export interface DynamicFiled {
    name: string;
    label: string;
    type: 'text' | 'number' | 'select';
    value?: any;
}

export interface DialogData {
    title: string;
    content?: string;
    type: 'infor' | 'confirm' | 'form',
    fields?: DynamicField[],
    confirmData?: string,
    cancelData?: string
}
