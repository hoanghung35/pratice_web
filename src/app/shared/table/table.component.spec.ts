import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { TableComponent } from './table.component';

interface Dummy {
  id: number;
  name: string;
}

describe('TableComponent', () => {
  let component: TableComponent<Dummy>;
  let fixture: ComponentFixture<TableComponent<Dummy>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableComponent, FormsModule]
    })
      .compileComponents();

    fixture = TestBed.createComponent<TableComponent<Dummy>>(TableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should slice data according to pageSize and currentPage', () => {
    component.pageSize = 2;
    component.data = [
      { id: 1, name: 'a' },
      { id: 2, name: 'b' },
      { id: 3, name: 'c' },
      { id: 4, name: 'd' }
    ];

    // first page
    component.currentPage = 0;
    expect(component.paginatedData.map(x => x.id)).toEqual([1, 2]);

    // second page
    component.currentPage = 1;
    expect(component.paginatedData.map(x => x.id)).toEqual([3, 4]);
  });

  it('should reset currentPage when data changes', () => {
    component.currentPage = 3;
    component.data = [{ id: 1, name: 'x' }];
    // ngOnChanges should set currentPage back to 0
    component.ngOnChanges({ data: { currentValue: component.data, firstChange: true, previousValue: null, isFirstChange: () => true } as any });
    expect(component.currentPage).toBe(0);
  });
});
