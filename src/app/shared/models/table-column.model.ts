import { TemplateRef } from '@angular/core';

export interface TableColumn<T> {
    id?: string;
    key?: keyof T;
    label: string;
    width?: string | number;
    render?: (row: T) => string;
    template?: TemplateRef<{ $implicit: T }>;
}
