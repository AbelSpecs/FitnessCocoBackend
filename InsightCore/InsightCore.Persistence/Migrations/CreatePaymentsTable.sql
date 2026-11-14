-- ============================================================
-- Script: CreatePaymentsTable
-- Proyecto: Pyrosfit / InsightCore
-- Descripción: Crea la tabla Payments para el módulo de
--              Gestión de Pagos multi-moneda (VES, USD, USDT)
-- ============================================================

-- ── 1. Crear tabla ────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS "Payments" (
    -- Clave primaria autoincremental (coincide con BaseEntity.Id → int)
    "Id"            SERIAL          PRIMARY KEY,

    -- FK al usuario que registra el pago (obligatorio)
    "UserId"        INTEGER         NOT NULL
                    REFERENCES "Users"("Id")
                    ON DELETE RESTRICT,

    -- FK al alumno vinculado al pago (opcional: puede ser pago de coach u otro)
    "StudentId"     INTEGER         NULL
                    REFERENCES "Students"("Id")
                    ON DELETE SET NULL,

    -- Monto con alta precisión: soporta montos grandes en Bs y 4 decimales para cripto
    "Amount"        NUMERIC(18, 4)  NOT NULL
                    CONSTRAINT CHK_Payments_Amount CHECK ("Amount" > 0),

    -- Moneda como string: 'VES' | 'USD' | 'USDT'
    "Currency"      VARCHAR(10)     NOT NULL,

    -- Método de pago como string legible
    "PaymentMethod" VARCHAR(30)     NOT NULL,

    -- Número de referencia bancaria, comprobante o hash de transacción cripto
    -- No es UNIQUE global porque un mismo hash puede usarse en distintos ambientes;
    -- la unicidad se valida en la lógica de negocio cuando sea necesario.
    "Reference"     VARCHAR(255)    NULL,

    -- Estado del pago
    "Status"        VARCHAR(20)     NOT NULL    DEFAULT 'Pendiente',

    -- Key del comprobante almacenado en Cloudflare R2 (solo la key, nunca la URL)
    "ReceiptKey"    VARCHAR(512)    NULL,

    -- Notas/comentarios adicionales (ej. motivo de rechazo)
    "Notes"         TEXT            NULL,

    -- Auditoría: timestamps UTC
    "CreatedAt"     TIMESTAMPTZ     NOT NULL    DEFAULT NOW(),
    "UpdatedAt"     TIMESTAMPTZ     NULL,

    -- Restricción de dominio para Currency
    CONSTRAINT CHK_Payments_Currency
        CHECK ("Currency" IN ('VES', 'USD', 'USDT')),

    -- Restricción de dominio para Status
    CONSTRAINT CHK_Payments_Status
        CHECK ("Status" IN ('Pendiente', 'Aprobado', 'Rechazado'))
);

-- ── 2. Índices para optimizar consultas frecuentes ────────────

-- Consultas por usuario (caso más común: "mis pagos")
CREATE INDEX IF NOT EXISTS "IX_Payments_UserId"
    ON "Payments" ("UserId");

-- Consultas por alumno (coach verifica pagos de sus alumnos)
CREATE INDEX IF NOT EXISTS "IX_Payments_StudentId"
    ON "Payments" ("StudentId");

-- Consultas por estado (listar pagos pendientes de revisión)
CREATE INDEX IF NOT EXISTS "IX_Payments_Status"
    ON "Payments" ("Status");

-- Índice compuesto para el filtro más habitual: usuario + estado + fecha
CREATE INDEX IF NOT EXISTS "IX_Payments_UserId_Status_CreatedAt"
    ON "Payments" ("UserId", "Status", "CreatedAt" DESC);

-- ── 3. Comentarios de columnas (documentación en BD) ─────────

COMMENT ON TABLE  "Payments"                IS 'Registro de pagos multi-moneda (VES/USD/USDT) del sistema Pyrosfit';
COMMENT ON COLUMN "Payments"."Amount"       IS 'Monto del pago. Precisión 18,4 para soportar VES y cripto';
COMMENT ON COLUMN "Payments"."Currency"     IS 'Moneda: VES (Bolívares), USD (Dólares), USDT (Tether)';
COMMENT ON COLUMN "Payments"."PaymentMethod" IS 'Método: Transferencia, PagoMovil, Zelle, BinancePay, Efectivo, PayPal, Otro';
COMMENT ON COLUMN "Payments"."Reference"    IS 'Número de referencia bancaria, comprobante o hash de transacción';
COMMENT ON COLUMN "Payments"."Status"       IS 'Estado: Pendiente (por verificar), Aprobado, Rechazado';
COMMENT ON COLUMN "Payments"."ReceiptKey"   IS 'Key del comprobante en Cloudflare R2. Nunca almacenar la URL completa';
