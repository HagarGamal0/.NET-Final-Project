# Lynx Talisman - Angular Integration Guide

To display the Lynx Talisman explanations in the Supplier/Admin panel, follow these steps.

## 1. models/assignment-observation.ts

Create a model interface:

```typescript
export interface AssignmentObservation {
  requestId: number;
  explanation: string;
  timestamp: string;
  decisionSource: 'SYSTEM' | 'SUPPLIER' | 'ADMIN';
}
```

## 2. Service Helper

Add this method to your `SupplierService` (or `LynxService`):

```typescript
getExplanation(requestId: number): Observable<AssignmentObservation> {
  return this.http.get<AssignmentObservation>(`${this.apiUrl}/Supplier/Explanation/${requestId}`);
}
```

## 3. UI Component (LynxExplanationComponent)

A simple read-only component to show the explanation.

```html
<div *ngIf="observation" class="p-4 bg-gray-50 border-l-4 border-indigo-500 rounded-r shadow-sm font-sans">
  <div class="flex items-center gap-2 mb-2">
    <span class="text-xl">🐆</span>
    <h3 class="text-sm font-bold text-gray-700 uppercase tracking-wide">The Lynx Talisman</h3>
  </div>
  
  <p class="text-gray-800 leading-relaxed text-sm">
    {{ observation.explanation }}
  </p>
  
  <div class="mt-3 flex gap-4 text-xs text-gray-500">
    <span>Source: {{ observation.decisionSource }}</span>
    <span>{{ observation.timestamp | date:'medium' }}</span>
  </div>
</div>
```
