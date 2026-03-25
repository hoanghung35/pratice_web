export interface DialogData {
    title: string;
    content?: string;
    type: 'info' | 'confirm' | 'form';
    fields?: DynamicField[]; // Dùng cho trường hợp có form
    confirmText?: string;
    cancelText?: string;
}

export interface DynamicField {
    label: string;
    name: string;
    type: 'text' | 'number' | 'select';
    options?: any[];
    value?: any;
}