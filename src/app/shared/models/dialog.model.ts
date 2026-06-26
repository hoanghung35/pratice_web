export interface DialogField {
    name: string;
    label: string;
    type: 'text' | 'number' | 'select';
    value?: any;
}

export interface DialogData {
    title: string;
    content?: string;
    type: 'infor' | 'confirm' | 'form',
    fields?: string,
    confirmData?: string,
    cancelData?: string
}
